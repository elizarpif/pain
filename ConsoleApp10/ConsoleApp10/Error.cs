using System;

namespace ConsoleApp10
{
    public class Error: Exception
    {
        private string message;
        public Error()
        {
            
        }

        public override string Message
        {
            get { return message; }
        }
    }
}