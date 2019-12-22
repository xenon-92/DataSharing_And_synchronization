using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UsingMutex
{
    class Mutex2
    {
        static void Main()
        {
            var tasks = new List<Task>();
            var ba1 = new BankAcct();
            var ba2 = new BankAcct();
            Mutex mutex1 = new Mutex();
            Mutex mutex2 = new Mutex();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool hasLock = mutex1.WaitOne();
                        try
                        {
                            ba1.Deposit(1);//10000
                        }
                        finally
                        {
                            if (hasLock)
                            {
                                mutex1.ReleaseMutex();
                            }
                        }
                        //Console.WriteLine($"-> ba1::: {ba1.Balance}");
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool hasLock = mutex2.WaitOne();
                        try
                        {
                            ba2.Deposit(1);//10000
                        }
                        finally
                        {
                            if (hasLock)
                            {
                                mutex2.ReleaseMutex();
                            }
                        }
                        //Console.WriteLine($"-> ba2::: {ba2.Balance}");

                    }
                }));
                tasks.Add(Task.Factory.StartNew(()=> {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool hasLock = WaitHandle.WaitAll(new[] { mutex1, mutex2 });
                        //it will wait for the mutex1 and mutex2 to get release
                        //only after successful release, it will perform transfer
                        try
                        {
                            ba1.Transfer(ba2, 1);
                        }
                        finally
                        {
                            if (hasLock)
                            {
                                mutex1.ReleaseMutex();
                                mutex2.ReleaseMutex();
                            }
                            
                        }
                        //Console.WriteLine($"--> ba1::: {ba1.Balance}");
                        //Console.WriteLine($"--> ba2::: {ba2.Balance}");

                    }
                }));
                //Console.WriteLine($"---> ba1::: {ba1.Balance}");
                //Console.WriteLine($"---> ba2::: {ba2.Balance}");
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine($"balance of ba1 is {ba1.Balance}");
            Console.WriteLine($"balance of ba2 is {ba2.Balance}");
            Console.ReadKey();
        }
    }
    class BankAcct
    {
        public int Balance { get; set; }
        public void Deposit(int amt)
        {
            Balance += amt;
        }
        public void Withdraw(int amt)
        {
            Balance -= amt;
        }
        public void Transfer(BankAcct where, int amount)
        {
            where.Balance += amount;
            Balance -= amount;

        }
    }
}
