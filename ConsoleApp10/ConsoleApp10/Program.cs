using System;

namespace ConsoleApp10
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Matrix matr = new Matrix(3);
            matr.setMatrixValues();
            Console.WriteLine($"determ: {matr.Determinant()}");
            matr.Print();
        }
    }
}