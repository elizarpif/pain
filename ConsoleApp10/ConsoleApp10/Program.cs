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
            float det = matr.Determinant();
            Console.WriteLine($"determ: {det}");
            // Console.WriteLine($"inverse_matrix: ");
            
            try
            {
                Matrix.TranspositionMatrix(matr).Print();
            }
            catch (MatrixException ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            matr.Print();
        }
    }
}