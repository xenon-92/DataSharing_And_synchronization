using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderWriterLock
{
    class WhyUpgradeReadLock
    {
        static void Main(string[] args)
        {
            ReaderWriterLockSlim rws = new ReaderWriterLockSlim();
            Random r = new Random();
            int x = 0;
            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    rws.EnterReadLock();
                    rws.EnterWriteLock();
                    //above line throws an exception
                    //Write lock may not be acquired with read lock held. This pattern is prone to deadlocks. 
                    //Please ensure that read locks are released before taking a write lock. 
                    //If an upgrade is necessary, use an upgrade lock in place of the read lock
                    if (i%2 ==0)
                    {
                        x = 123;
                    }
                    rws.ExitWriteLock();
                    Console.WriteLine("Entering read lock..");
                    Console.WriteLine($"value of x in readlock is {x}");
                    Thread.Sleep(5000);
                    rws.ExitReadLock();
                    Console.WriteLine("Exited readlock....");
                }));
            }
            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {

                ae.Handle(e => {
                    Console.WriteLine(e);
                    return true;
                });
            }
            int k = 10;
            while (k-- > 0)
            {
                rws.EnterWriteLock();
                Console.WriteLine("Entered write lock");
                int newValue = r.Next(10);
                x = newValue;
                Console.WriteLine($"value of x after entering writelock is {x}");
                Console.WriteLine($"exiting writelock");
                rws.ExitWriteLock();
            }
            Console.ReadKey();
        }
    }
}
