using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppBank
{
    public class BankAccount
    {
        public int Account;
        private string PIN;
        private double Balance;

        public int account
        {
            get {  return Account; }
        }
        public string pin
        {
            get { return PIN; }
        }
        public double balance
        {
            get { return Balance; }
        }

        public BankAccount(string code)
        {
            if (Validate(code))
            {
                this.PIN = Hash(code);
                this.Balance = 0;
                GenerateAccountNumber();
            }
        }
        private bool Validate(string PIN)
        {
            bool isNumber = int.TryParse(PIN, out _);
            if (PIN.Length == 4 && isNumber)
                return true;
            else
                return false;
        }
        private string Hash(string PIN)
        {
            using (SHA512 hash = SHA512.Create())
            {
                //Console.WriteLine(Convert.ToHexString(hash.ComputeHash(Encoding.ASCII.GetBytes(PIN))));
                return Convert.ToHexString(hash.ComputeHash(Encoding.ASCII.GetBytes(PIN)));
            }
        }
        public void Replenishment(double amount)
        {
            this.Balance += amount;
            Console.WriteLine($"Баланс пополнен на сумму: {amount}");
        }
        private void GenerateAccountNumber()
        {
            Random rand = new Random();
            this.Account = rand.Next(100000, 1000000); //так как Account имеет тип int вначале не может быть 0
        }
    }

    //Основной функционал
    public class Bank
    {
        public List<BankAccount> BankAccounts = new List<BankAccount>();

        public void PrintAccounts()
        {
            Console.WriteLine();
            Console.WriteLine("Все счета: ");
            foreach (BankAccount account in BankAccounts)
            {
                Console.WriteLine($"Счет: {account.account}");
            }
            Console.WriteLine();
        }

        private string GeneratePINCode()
        {
            Random rand = new Random();
            return $"{rand.Next(0, 10)}{rand.Next(0, 10)}{rand.Next(0, 10)}{rand.Next(0, 10)}";
        }

        public void AddAccount()
        {
            Console.Write("Введите 1 - если хотите вручную задать пин-код, 2 - если хотите сгенерировать пин-код: ");
            string choice = Console.ReadLine();
            switch(choice)
            {
                case "1":
                    {
                        Console.Write("Введите пин-код: ");
                        string code = Console.ReadLine();
                        BankAccount newAcc = new BankAccount(code);
                        BankAccounts.Add(newAcc);
                        break;
                    }
                case "2":
                    {
                        string code = GeneratePINCode();
                        Console.WriteLine($"Ваш код: {code}");
                        BankAccount newAcc = new BankAccount(code);
                        BankAccounts.Add(newAcc);
                        break;
                    }
            }
        }

        public void CheckBalance(int account)
        {
            BankAccount? accFind = BankAccounts.Find(acc => acc.account == account);
            if( accFind != null )
            {
                Console.WriteLine($"Баланс счета '{account}': {accFind.balance}");
            }
            else
            {
                Console.WriteLine("Такого счета не существует!");
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Bank bank = new Bank();
            bank.AddAccount();
            bank.PrintAccounts();
            Console.Write("Введите номер счета: ");
            int account = Convert.ToInt32(Console.ReadLine());
            bank.CheckBalance(account);
        }
    }
}
