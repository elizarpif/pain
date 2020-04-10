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
    public class Matrix : ICloneable
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
            for (int i = 0; i < Size; i++)
            {
                List<float> strValues = new List<float>();
                for (int j = 0; j < Size; j++)
                {
                    strValues.Add(0);
                }

                values.Add(strValues);
            }
        }

        public Matrix()
        {
            Size = 0;
            values = new List<List<float>>();
        }

        public void setMatrixValues()
        {
            if (Size == 0 && !SetSize())
            {
                return;
            }
            
            setValues();
        }

        public object Clone()
        {
            Matrix m = new Matrix();
            m.Size = this.Size;
            m.values = new List<List<float>>();
            foreach (List<float> str in values)
            {
                List<float> valuesInStr = new List<float>();
                foreach (float val in str)
                {
                    valuesInStr.Add(val);
                }

                m.values.Add(valuesInStr);
            }

            return m;
        }

        public float Determinant()
        {
            int i, j, k, minus = 0, size = Size;
            float h, max, min, tmp, tmp2, opr = 1;

            Matrix m = (Matrix) this.Clone();

            for (i = 0; i < size; i++)
            {
                tmp = m.values[i][i];
                if (tmp.Equals(0)) // если текущий элемент 0
                {
                    for (j = i + 1; j < size; j++)
                    {
                        tmp2 = m.values[j][i];
                        if (!tmp2.Equals(0))
                        {
                            for (k = 0; k < size; k++) // меняем местами строки
                            {
                                h = m.values[j][k];
                                m.values[j][k] = m.values[i][k];
                                m.values[i][k] = h;

                                minus += 1;
                            }
                        }
                    }
                }
                else //если текущий не 0
                {
                    for (k = i + 1; k < size; k++)
                    {
                        max = m.values[k][i]; //max - делимое, min - делитель
                        min = m.values[i][i];
                        h = max / min; //делим, чтобы получить единички

                        for (j = i; j < size; j++)
                        {
                            m.values[k][j] = m.values[k][j] - h * m.values[i][j];
                        }
                    }
                }
            }

            int edin = 0;
            while (edin < size - 1)
            {
                for (i = 0; i < size; i++)
                {
                    tmp = m.values[i][i];

                    if (tmp.Equals(0))
                        opr = 0;

                    if (tmp.Equals(1))
                        edin++;
                }

                edin++;
            }

            if (!opr.Equals(0))
                opr = 1;
            else
                return opr;

            for (i = 0; i < size; i++)
                opr *= m.values[i][i];

            if (minus % 2 != 0)
                opr *= (-1);

            return opr;
        }

        // public Matrix InverseMatrix(float det)
        // {
        // }

        //транспонирование
        public static Matrix TranspositionMatrix(Matrix a)
        {
            Matrix resMass = new Matrix(a.Size);
            for (int i = 0; i < a.Size; i++)
            {
                for (int j = 0; j < a.Size; j++)
                {
                    resMass.values[i][j] = a.values[j][i];
                }
            }

            return resMass;
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