using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MutexInPlay
{
    class ConcurrentMutex
    {
        static void Main()
        {
            var tasks = new List<Task>();
            var ba = new BankAccountConcurrent();
            var ba1 = new BankAccountConcurrent();
            Mutex m = new Mutex();
            Mutex m1 = new Mutex();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(()=> {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool lockTaken = m.WaitOne();
                        try
                        {
                            ba.Deposit(1);
                        }
                        finally
                        {
                            if (lockTaken)
                            {
                                m.ReleaseMutex();
                            }
                        }
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() => {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool lockTaken = m1.WaitOne();
                        try
                        {
                            ba1.Deposit(1);
                        }
                        finally
                        {
                            if (lockTaken)
                            {
                                m1.ReleaseMutex();
                            }
                        }
                    }
                }));
                tasks.Add(Task.Factory.StartNew(()=> {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool hasLock = WaitHandle.WaitAll(new[] { m1,m});
                        try
                        {
                            ba1.Transfer(ba,1);
                        }
                        finally
                        {
                            if (hasLock)
                            {
                                m1.ReleaseMutex();
                                m.ReleaseMutex();
                            }
                        }
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine(ba.Balance);
            Console.WriteLine(ba1.Balance);
            Console.ReadKey();
        }
    }
    class BankAccountConcurrent
    {
        public int Balance { get; set; }
        public void Withdraw(int amt)
        {
            Balance -= amt;
        }
        public void Deposit(int amt)
        {
            Balance += amt;
        }
        public void Transfer(BankAccountConcurrent where,int amt)
        {
            where.Balance += amt;
            Balance -= amt;
        }
    }
}
