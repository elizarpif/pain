using System;
using System.Collections.Generic;
using System.Text;


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
    public class Matrix : ICloneable, IComparable, IComparable<Matrix> 
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
            values = Matrix.CustomValues(n, n);
        }


        // CustomValues создает список списков размера n * m, заполненный нулями
        private static List<List<float>> CustomValues(int n, int m)
        {
            List<List<float>> values = new List<List<float>>();
            for (int i = 0; i < n; i++)
            {
                List<float> strValues = new List<float>();
                for (int j = 0; j < m; j++)
                {
                    strValues.Add(0);
                }

                values.Add(strValues);
            }

            return values;
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

        //обратная матрица
        public static Matrix InverseMatrix(Matrix a)
        {
            Matrix resMass = new Matrix(a.Size);
            Matrix algAddMatrix = new Matrix(a.Size); // алгебр дополнение
            if (Math.Abs(a.Determinant()).Equals(0))
            {
                throw new MatrixException("Определитель матрицы равен 0. Матрицa вырожденная");
            }

            for (int i = 0; i < a.Size; i++)
            {
                for (int j = 0; j < a.Size; j++)
                {
                    algAddMatrix.values[i][j] = (float) (Math.Pow(-1, i + j) * Minor(i, j, a).Determinant());
                }
            }

            algAddMatrix = TranspositionMatrix(algAddMatrix);
            resMass = CompositionWhithNum(algAddMatrix, 1 / a.Determinant());
            return resMass;
        }

        // Умножение матрицы А на число
        public static Matrix CompositionWhithNum(Matrix a, float num)
        {
            Matrix resMass = new Matrix(a.Size);
            for (int i = 0; i < a.Size; i++)
            {
                for (int j = 0; j < a.Size; j++)
                {
                    resMass.values[i][j] = a.values[i][j] * num;
                }
            }

            return resMass;
        }

        //миноры
        public static Matrix Minor(int row, int col, Matrix a)
        {
            Matrix resMass = new Matrix(a.Size - 1);
            for (int i = 0; i < a.Size - 1; i++)
            {
                if (i < row)
                {
                    for (int j = 0; j < a.Size - 1; j++)
                    {
                        if (j < col)
                        {
                            resMass.values[i][j] = a.values[i][j];
                        }
                        else
                        {
                            resMass.values[i][j] = a.values[i][j + 1];
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < a.Size - 1; j++)
                    {
                        if (j < col)
                        {
                            resMass.values[i][j] = a.values[i + 1][j];
                        }
                        else
                        {
                            resMass.values[i][j] = a.values[i + 1][j + 1];
                        }
                    }
                }
            }

            return resMass;
        }


        //транспонирование
        public static Matrix TranspositionMatrix(Matrix a)
        {
            Matrix resMass = new Matrix(a.Size);
            for (int i = 0;
                i < a.Size;
                i++)
            {
                for (int j = 0; j < a.Size; j++)
                {
                    resMass.values[i][j] = a.values[j][i];
                }
            }

            return resMass;
        }

        public int getSize
            => this.Size;

        public static Matrix SumMatrix(Matrix a, Matrix b)
        {
            if (a.Size != b.Size)
            {
                throw new MatrixException("Размерности матриц не совпадают");
            }

            Matrix resMass = new Matrix(a.Size);
            for (int i = 0;
                i < a.Size;
                i++)
            {
                for (int j = 0; j < b.Size; j++)
                {
                    resMass.values[i][j] = a.values[i][j] + b.values[i][j];
                }
            }

            return resMass;
        }

        // Умножение матрицы А на матрицу B
        public static Matrix CompositionMatrix(Matrix a, Matrix b)
        {
            if (a.Size != b.Size)
            {
                throw new MatrixException("Эти матрицы нельзя перемножить");
            }

            Matrix resMass = new Matrix(a.Size);
            for (int i = 0;
                i < a.Size;
                i++)
            for (int j = 0;
                j < b.Size;
                j++)
            for (int k = 0;
                k < b.Size;
                k++)
                resMass.values[i]
                    [j] += a.values[i]
                    [k] * b.values[k]
                    [j];
            return resMass;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            return CompositionMatrix(a, b);
        }


        public static Matrix operator +(Matrix a, Matrix b)
        {
            return SumMatrix(a, b);
        }

        //деление матрицы A на матрицу B
        public static Matrix DivisionMatrix(Matrix a, Matrix b)
        {
            return Matrix.CompositionMatrix(a, Matrix.InverseMatrix(b));
        }

        public static Matrix operator /(Matrix a, Matrix b)
        {
            return DivisionMatrix(a, b);
        }
        
        //возведение матрицы в степень
        public static Matrix Pow(Matrix A, int pow) {
            var resultMatrix = A;
            for (int i = 0; i < pow - 1; i++) {
                resultMatrix *= A;
            }
            return resultMatrix;
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            double eps = Math.Pow(10, -3);
            
            foreach (var list in values)
            {
                foreach (var value in list)
                {
                    if (value < eps)
                        str.Append("0 ");
                    else
                        str.Append($"{value} ");
                }

                str.Append("\n");
            }

            return str.ToString();
        }


        private void setValues()
        {
            List<List<float>> res = new List<List<float>>();

            Console.WriteLine($"введите значения матрицы ({Size} x {Size}) через пробел: ");

            try
            {
                for (int i = 0; i < Size; i++)
                {
                    string[] vals = Console.ReadLine().Split(' ');

                    List<float> valuesInStr = new List<float>();

                    foreach (string val in vals)
                    {
                        float v = float.Parse(val);
                        valuesInStr.Add(v);
                    }

                    res.Add(valuesInStr);
                }
            }
            catch (FormatException ex)
            {
                Console.WriteLine("ошибка при чтении значений; выход");
                Environment.Exit(Environment.ExitCode);
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

        public int CompareTo(Matrix A) {
            var ca1 = this.Determinant();
            var ca2 = A.Determinant();
            if (ca1 > ca2) {
                return 1;
            } else if (ca1 < ca2) {
                return -1;
            } else {
                return 0;
            }
        }
        public int CompareTo(object obj) {
            if (!(obj is Matrix)) {
                throw new MatrixException("Не могу сравнить значения");
            }
            if (obj is null) {
                throw new MatrixException("NULL");
            }
            return CompareTo((Matrix)obj);
        }
    }
}