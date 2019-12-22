using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MutexInPlay
{
    class SharingSameMutexInDifferentInstance
    {
        static void Main()
        {
            const string appName = "demo";
            Mutex mutex;//global mutex
            try
            {
                //try and get a global mutex
                mutex = Mutex.OpenExisting(appName);
                Console.WriteLine($"sorry but the program is already, running : {appName}");


            }
            catch (WaitHandleCannotBeOpenedException e)
            {

                Console.WriteLine("The application is running just fine, and no body has started it....");
                mutex = new Mutex(false, appName);
            }
            Console.ReadKey();
            mutex.ReleaseMutex();
        }
    }
}
