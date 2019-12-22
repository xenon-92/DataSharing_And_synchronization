using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Inter_locked
{
    //Mostly applicable to variables of primitive type
    //performs an operation atomically
    class Program
    {
        static void Main(string[] args)
        {
            var ba = new BankAccount();
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
            Console.WriteLine(ba.Balance);
            Console.ReadKey();
        }
    }
    class BankAccount
    {
        private int balance;
        public int Balance 
        {
            get { return balance; } 
            set {value = balance;} 
        }
        public void Deposit(int amt)
        {
            Interlocked.Add(ref balance, amt);
        }
        public void Withdraw(int amt)
        {
            Interlocked.Add(ref balance,-amt);
        }
    }
}
