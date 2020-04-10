using System;

namespace ConsoleApp10
{
    class MatrixException : Exception
    {
        public MatrixException(string message)
            : base(message)
        { }
    }
}