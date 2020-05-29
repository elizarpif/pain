using System;

namespace ConsoleApplication15
{
    public class ObservationException : Exception
    {
        private static string message = "observation exception";

        public ObservationException(string message)
            : base(message)
        {
        }

        public ObservationException()
            : base(message)
        {
        }
    }
}