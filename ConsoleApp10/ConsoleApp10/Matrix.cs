using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace ConsoleApp10
{
    /*
         Реализовать класс квадратной матрицы произвольного размера и для него
    операции сложения, умножения, деления, нахождение определителя и
    транспонирования. При невозможности выполнить какую-либо операцию
    генерируйте исключение (Объект исключение должен быть объектом
    пользовательского класса, пронаследованного от Exception). Реализовать интерфейс
    ICloneable. Добавить возможность работы с элементами матрицы в цикле foreach.
    Примечание. Класс должен содержать не менее 3 различных конструкторов.
     */
    public class Matrix
    {
        private int Size;
        private List<List<float>> values;

        public Matrix(List<List<float>> vals)
        {
            int size = vals.Count;
            bool check = true;
            foreach (var list in vals)
            {
                int rowSize = list.Count;
                if (rowSize != size)
                {
                    check = false;
                    break;
                }
                    
            }

            if (check)
            {
                values = vals;
                Size = size;
            }
            else
            {
                Size = 0;
                values = new List<List<float>>();
            }

        }
        public Matrix(int n)
        {
            Size = n;
            values = new List<List<float>>();
        }

        public Matrix()
        {
            Size = 0;
            values = new List<List<float>>();
        }

        public void setMatrixValues()
        {
            if (values.Count == 0)
            {
                if (Size == 0 && !SetSize())
                {
                    return;
                }
                setValues();
            }
        }

        public void Print()
        {
            Console.WriteLine($"текущая матрица ({Size} x {Size}):");

            foreach (var list in values)
            {
                foreach (var value in list)
                {
                    Console.Write($"{value} ");
                }
                Console.WriteLine();
            }
        }
        private void setValues()
        {
            List<List<float>> res = new List<List<float>>();
            
            Console.WriteLine($"введите значения матрицы ({Size} x {Size}) через пробел: ");
            
            for (int i = 0; i < Size; i++)
            {
                string[] vals = Console.ReadLine().Split(' ');
                
                List<float> valuesInStr = new List<float>();
                
                try
                {
                    foreach (string val in vals)
                    {
                        float v = float.Parse(val);
                        valuesInStr.Add(v);
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine("ошибка при чтении значений; выход");
                    return;
                }
                
                res.Add(valuesInStr);
            }

            values = res;
        }
        private bool SetSize()
        {
            while (Size == 0)
            {
                Console.WriteLine("введите размер матрицы ( > 0): ");
                string sizeStr = Console.ReadLine();
                
                if (sizeStr == "exit")
                    break;
                
                try
                {
                    Size = Int32.Parse(sizeStr);
                }
                catch (FormatException e)
                {
                    Console.WriteLine("ошибка - размер матрицы не может быть не числом");
                    Console.WriteLine("попробуйте еще раз. для выхода введите exit");
                }
            }

            if (Size == 0)
                return false;
            return true;
        }
    }
}