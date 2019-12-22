using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Critical_Section
{
    class Fix_CriticalSection
    {
        static void Main()
        {
            var ba = new BankBalnce();
            var tasks = new List<Task>();
            for (int i = 0; i < 1000; i++)
            {
                tasks.Add(Task.Factory.StartNew(()=> {
                    for (int j = 0; j < 100; j++)
                    {
                        ba.Deposit(100);
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() => {
                    for (int j = 0; j < 100; j++)
                    {
                        ba.Withdraw(100);
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine(ba.Balance);
            Console.ReadKey();
        }
    }
    class BankBalnce
    {
        private readonly object padlock = new object();
        public int Balance { get; set; }
        public void Deposit(int amt)
        {
            lock (padlock)
            {
                Balance += amt;
            }
        }
        public void Withdraw(int amt)
        {
            lock (padlock)
            {
                Balance -= amt;
            }
        }
    }
}
