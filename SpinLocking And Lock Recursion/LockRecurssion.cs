using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SpinLocking_And_Lock_Recursion
{
    class LockRecurssion
    {
        static SpinLock sl = new SpinLock(true); //true is used for debugging purpose

        static void Main()
        {
            LockRecusrion(5);
        }
        static void LockRecusrion(int x)
        {
            bool lockTaken = false;
            try
            {
                sl.Enter(ref lockTaken);
            }
            catch (LockRecursionException e)//expection a lock recurssion exception
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (lockTaken)
                {
                    Console.WriteLine($"Lock is being taken, x= {x}");
                    LockRecusrion(x-1);
                    sl.Exit();
                }
                else
                {
                    Console.WriteLine($"Failed to take lock, x= {x}");
                }
            }
            Console.ReadKey();
        }
    }
}
