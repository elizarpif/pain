using System;
using System.Drawing;
using System.Windows.Markup;

namespace ConsoleApp10
{
    class Program
    {
        static void Main(string[] args)
        {
            Matrix matr = new Matrix(3);

            try
            {
                matr.setMatrixValues();
            
                float det = matr.Determinant();
                Console.WriteLine($"determ: {det}");
                
                /*Matrix m = Matrix.TranspositionMatrix(matr);
                Console.WriteLine("transposition ");
                m.Print();
                
                Matrix sum = m + matr;
                Console.WriteLine("sum ");
                sum.Print();

                Matrix mult = m * matr;
                Console.WriteLine("multiply ");
                mult.Print();*/

                Matrix inverse = Matrix.InverseMatrix(matr);
                Console.WriteLine($"inverse_matrix: ");
                inverse.Print();
                
                Matrix div = matr / matr;
                div.Print();

            }
            catch (MatrixException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}