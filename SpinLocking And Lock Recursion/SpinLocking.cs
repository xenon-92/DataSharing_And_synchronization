using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpinLocking_And_Lock_Recursion
{
    class SpinLocking
    {
        // spinlock is same as padlock but the spinlock decoration is embedded on
        //the calling method rather than in the called method [for paddlock]
        static void Main(string[] args)
        {
            var ba = new BankBalance();
            var tasks = new List<Task>();
            SpinLock sl = new SpinLock(); //make new instance of spinlock class
            for (int i = 0; i < 1000; i++)
            {
                tasks.Add(Task.Factory.StartNew(()=> {
                    for (int j = 0; j < 100; j++)
                    {
                      bool lockTaken = false; // declare a new bool varaible as false, which has to be passed by ref
                      try
                      {
                          sl.Enter(ref lockTaken);//try and accquire spin lock, with reference to boolean variable
                        
                          //if we are successful in accquiring the lock, then we can deposit the money
                          ba.Deposit(100);

                      }
                      finally
                      {
                            //if we were successful in accuring the lock 
                          if (lockTaken)
                          {
                                //then release the lock
                              sl.Exit();
                          }
                      }
                    }
                }));
                tasks.Add(Task.Factory.StartNew(() => {
                    for (int j = 0; j < 100; j++)
                    {
                        bool lockTaken = false;
                        try
                        {
                            sl.Enter(ref lockTaken);
                            ba.Withdraw(100);
                        }
                        finally
                        {
                            if (lockTaken)
                            {
                                sl.Exit();
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
    class BankBalance
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
    }
}
