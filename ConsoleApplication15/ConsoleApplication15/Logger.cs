using System;
using System.Threading;

namespace ConsoleApplication15
{
    public class Logger
    {
        private static Mutex mutex;

        public Logger()
        {
            mutex = new Mutex();
        }

        public void PrintBlack(string s)
        {
            Print(s, false);
        }

        private void Print(string s, bool isRed)
        {
            mutex.WaitOne();

            if (isRed)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(s);
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.WriteLine(s);
            }

            mutex.ReleaseMutex();
        }

        public void PrintRed(string s)
        {
            Print(s, true);
        }
    }
}