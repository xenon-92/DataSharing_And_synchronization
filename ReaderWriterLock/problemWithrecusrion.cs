﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderWriterLock
{
    class problemWithrecusrion
    {
        static void Main(string[] args)
        {
            ReaderWriterLockSlim rws = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
            Random r = new Random();
            int x = 0;
            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    rws.EnterReadLock();
                    rws.EnterReadLock();//if we successively try to acquire readlock before releasing the previous error it throws the exception

                    Console.WriteLine("Entering read lock..");
                    Console.WriteLine($"value of x in readlock is {x}");
                    Thread.Sleep(5000);
                    rws.ExitReadLock();
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
