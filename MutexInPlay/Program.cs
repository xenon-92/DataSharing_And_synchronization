using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MutexInPlay
{
    class Program
    {
        static void Main(string[] args)
        {
            var tasks = new List<Task>();
            var ba = new BankAccount();
            Mutex mutex = new Mutex();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool hasTaken = mutex.WaitOne();
                        try
                        {
                            ba.Withdraw(100);
                        }
                        finally
                        {
                            if (hasTaken)
                            {
                                mutex.ReleaseMutex();
                            }
                        }
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool hasTaken = mutex.WaitOne();
                        try
                        {
                            ba.Deposit(100);
                        }
                        finally
                        {
                            if (hasTaken)
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
        private int balance;
        public int Balance { get { return balance; } set { value = balance; } }
        public void Withdraw(int amt)
        {
            balance -= amt;
        }
        public void Deposit(int amt)
        {
            balance += amt;
        }
    }
}
