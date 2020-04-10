using System;
using System.Drawing;
using System.Windows.Markup;

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
                Matrix m = Matrix.TranspositionMatrix(matr);
                Console.WriteLine("transposition ");
                m.Print();
                
                Matrix sum = m + matr;
                Console.WriteLine("sum ");
                sum.Print();

                Matrix mult = m * matr;
                Console.WriteLine("multiply ");
                mult.Print();
            }
            catch (MatrixException ex)
            {
                Console.WriteLine(ex.Message);
            }
            
           // matr.Print();
        }
    }
}