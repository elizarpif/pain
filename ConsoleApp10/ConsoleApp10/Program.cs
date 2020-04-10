using System;
using System.Drawing;
using System.Windows.Markup;

namespace ConsoleApp10
{
    class Program
    {
        static void Main(string[] args)
        {
            Matrix matr = new Matrix(2);

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
                Console.WriteLine($"inverse_matrix: {inverse.ToString()}");
                
                var a = new Polynom<Matrix>(1, matr, matr);
                var b = new Polynom<Matrix>(1, matr, inverse);
                var c = a * b;
            
                Console.WriteLine(c.ToString());
            }
            catch (MatrixException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}