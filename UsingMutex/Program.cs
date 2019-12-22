using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace UsingMutex
{
    class Program
    {
        static void Main(string[] args)
        {
            var ba = new BankAccount();
            var tasks = new List<Task>();
            Mutex mutex = new Mutex();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 100; j++)
                    {
                        bool hasLock = mutex.WaitOne();
                        try
                        {   
                            ba.Deposit(100);
                        }
                        finally
                        {
                            if (hasLock)
                            {
                                mutex.ReleaseMutex();
                            }
                        }

                    }
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 100; j++)
                    {
                        bool hasMutex = mutex.WaitOne();
                        try
                        {
                            ba.Withdraw(100);
                        }
                        finally
                        {
                            if (hasMutex)
                            {
                                mutex.ReleaseMutex();
                            }
                        }
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine(ba.Balance);
            Console.ReadKey();
        }
    }
    class BankAccount
    {
        int balance;
        public int Balance
        {
            get
            {
                return balance;
            }
            set
            {
                value = balance;
            }
        }
        public void Deposit(int amount)
        {
            balance += amount;
        }
        public void Withdraw(int amount)
        {
            balance -= amount;
        }
    }
}
