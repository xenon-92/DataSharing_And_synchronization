using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Critical_Section
{
    //the below solution does not produce a 0 as final balance 
    // due to the race conditions, when a thread tries to access a value 
    //maybe another thread has changed the value
    class Program
    {
        static void Main(string[] args)
        {
            var ba = new BankAccount();
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 1000; i++)
            {
                tasks.Add(Task.Factory.StartNew(()=> {
                    for (int j = 0; j < 100; j++)
                    {
                        ba.Deposit(100);
                    }
                }));
                tasks.Add(Task.Factory.StartNew(()=> {
                    for (int k = 0; k < 100; k++)
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
     class BankAccount
    {
        public int Balance { get; set; }
        public void Deposit(int amount)
        {
            Balance += amount;
        }
        public void Withdraw(int amount)
        {
            Balance -= amount;
        }
    }
}
