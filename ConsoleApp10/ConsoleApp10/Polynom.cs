using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;

namespace ConsoleApp10
{
    class Polynom<T> : ICloneable, IEnumerable<T>, IComparable, IComparable<Polynom<T>>
        where T : IComparable, new()
    {
        private Dictionary<int, T> pairDegreeCoef = new Dictionary<int, T>();

        public Polynom()
        {
        }
        
        public Polynom(int power, params T[] coefficientArray)
        {
            if (power > coefficientArray.Length - 1)
                throw new ArgumentException("степень не соответствует количеству элементов");

            for (int i = power; i >= 0; i--)
            {
                pairDegreeCoef.Add(i, coefficientArray[power - i]);
            }
        }

        public Polynom(T[] coefficientArray)
        {
            for (int i = coefficientArray.Length - 1; i >= 0; i--)
            {
                pairDegreeCoef.Add(i, coefficientArray[coefficientArray.Length - 1 - i]);
            }
        }

        public Polynom(Dictionary<int, T> dict)
        {
            foreach (var KVPair in dict)
            {
                pairDegreeCoef.Add(KVPair.Key, KVPair.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return pairDegreeCoef.Values.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return pairDegreeCoef.Values.GetEnumerator();
        }

        public object Clone()
        {
            Dictionary<int, T> cloneDictionary = new Dictionary<int, T>(pairDegreeCoef.Count);
            foreach (var KVPair in pairDegreeCoef)
            {
                cloneDictionary.Add(KVPair.Key, KVPair.Value);
            }

            return cloneDictionary;
        }

        public int CompareTo(Polynom<T> A)
        {
            return pairDegreeCoef.Keys.Max().CompareTo(A.pairDegreeCoef.Keys.Max());
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException("NULL", nameof(obj));
            }

            if (!(obj is Polynom<T>))
            {
                throw new ArgumentException("This is not a polynom class", nameof(obj));
            }

            return CompareTo((Polynom<T>) obj);
        }

        /*
         * Generates a new zeros dictionary 
        */
        private static Dictionary<int, T> Zeros(Polynom<T> A)
        {
            var resultDictionary = new Dictionary<int, T>(A.pairDegreeCoef.Count);
            foreach (var KVPair in A.pairDegreeCoef)
            {
                resultDictionary.Add(KVPair.Key, new T());
            }

            return resultDictionary;
        }
        
        private static Polynom<T> GetMaxPolynome(Polynom<T> A, Polynom<T> B)
        {
            if (A.CompareTo(B) < 0)
            {
                return B;
            }
            else
            {
                return A;
            }
        }
        
        private static Polynom<T> GetMinPolynome(Polynom<T> A, Polynom<T> B)
        {
            if (A.CompareTo(B) <= 0)
            {
                return A;
            }
            return B;
            
        }

        public static Polynom<T> PolynomSum(Polynom<T> A, Polynom<T> B)
        {
            var maxPolynome = GetMaxPolynome(A, B);
            var resultPolynome = new Polynom<T>(Zeros(maxPolynome));

            for (int i = 0; i < maxPolynome.pairDegreeCoef.Count; i++)
            {
                if (GetMinPolynome(A, B).pairDegreeCoef.ContainsKey(i))
                {
                    resultPolynome.pairDegreeCoef[i] = (dynamic) A.pairDegreeCoef[i] + B.pairDegreeCoef[i];
                }
                else
                {
                    resultPolynome.pairDegreeCoef[i] = maxPolynome.pairDegreeCoef[i];
                }
            }

            return resultPolynome;
        }

        public static Polynom<T> operator +(Polynom<T> A, Polynom<T> B)
        {
            return PolynomSum(A, B);
        }
        
        public static Polynom<T> PolynomeDifference(Polynom<T> A, Polynom<T> B)
        {
            var maxPolynome = GetMaxPolynome(A, B);
            var resultPolynome = new Polynom<T>(Zeros(maxPolynome));

            for (int i = 0; i < maxPolynome.pairDegreeCoef.Count; i++)
            {
                if (!A.pairDegreeCoef.ContainsKey(i))
                {
                    resultPolynome.pairDegreeCoef[i] = (dynamic) (-1) * B.pairDegreeCoef[i];
                }
                else if (!B.pairDegreeCoef.ContainsKey(i))
                {
                    resultPolynome.pairDegreeCoef[i] = A.pairDegreeCoef[i];
                }
                else
                {
                    resultPolynome.pairDegreeCoef[i] = (dynamic) A.pairDegreeCoef[i] - (dynamic) B.pairDegreeCoef[i];
                }
            }

            return resultPolynome;
        }

        public static Polynom<T> operator -(Polynom<T> A, Polynom<T> B)
        {
            return PolynomeDifference(A, B);
        }

        //произведение полиномов
        public static Polynom<T> PolynomeComposition(Polynom<T> A, Polynom<T> B)
        {
            var resultPolynome = new Polynom<T>();
            for (int i = 0; i < A.pairDegreeCoef.Count; i++)
            {
                for (int j = 0; j < B.pairDegreeCoef.Count; j++)
                {
                    if (resultPolynome.pairDegreeCoef.ContainsKey(i + j))
                    {
                        resultPolynome.pairDegreeCoef[i + j] += (dynamic) A.pairDegreeCoef[i] * B.pairDegreeCoef[j];
                    }
                    else
                    {
                        resultPolynome.pairDegreeCoef.Add(i + j,
                            (dynamic) A.pairDegreeCoef[i] * B.pairDegreeCoef[j]);
                    }
                }
            }

            return resultPolynome;
        }

        public static Polynom<T> operator *(Polynom<T> A, Polynom<T> B)
        {
            return PolynomeComposition(A, B);
        }

        //умножение полинома на число
        public static Polynom<T> PolynomeCompositionWithNum(Polynom<T> A, T num)
        {
            var resultPolynome = new Polynom<T>();
            for (int i = 0; i < A.pairDegreeCoef.Count; i++)
            {
                resultPolynome.pairDegreeCoef[i] = (dynamic) A.pairDegreeCoef[i] * num;
            }

            return resultPolynome;
        }

        public static Polynom<T> operator *(Polynom<T> A, T num)
        {
            return PolynomeCompositionWithNum(A, num);
        }

        //возведение полинома в степень
        public static Polynom<T> PolynomePow(Polynom<T> A, int pow)
        {
            Polynom<T> resultPolynom = A;
            for (int i = 1; i < pow; i++)
            {
                resultPolynom *= A;
            }

            return resultPolynom;
        }

        //значение полинома в точке
        public T PolynomeInDot(T dot)
        {
            T result = new T();
            for (int i = 0; i < pairDegreeCoef.Count; i++)
            {
                if (dot is Matrix)
                {
                    result += Matrix.Pow((dynamic) dot, i) * pairDegreeCoef[i];
                }
                else
                {
                    result += Math.Pow((dynamic) dot, i) * pairDegreeCoef[i];
                }
            }

            return result;
        }

        //композиция полиномов
        public static Polynom<T> CompositionPolynimWithPolynome(Polynom<T> A, Polynom<T> B)
        {
            var resultPolynome = new Polynom<T>(Zeros(A));
            var polynomArr = new Polynom<T>[A.pairDegreeCoef.Count];
            for (int i = 1; i < polynomArr.Length; i++)
            {
                polynomArr[i] =
                    new Polynom<T>((dynamic) PolynomeCompositionWithNum(PolynomePow(B, i), A.pairDegreeCoef[i])
                        .pairDegreeCoef);
            }

            for (int i = 1; i < polynomArr.Length; i++)
            {
                resultPolynome += (dynamic) polynomArr[i];
            }

            resultPolynome.pairDegreeCoef[0] = (dynamic) resultPolynome.pairDegreeCoef[0] + A.pairDegreeCoef[0];
            return resultPolynome;
        }

        //вывод
        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            for (int i = pairDegreeCoef.Count - 1; i >= 0; i--)
            {
                if (pairDegreeCoef[i] is Matrix)
                {
                    if (i == 0)
                    {
                        str.Append($"{pairDegreeCoef[i]} * x^{i}{Environment.NewLine}");
                    }
                    else
                    {
                        str.Append($"{pairDegreeCoef[i]} * x^{i} + {Environment.NewLine}");
                    }
                }
                else
                {
                    if (pairDegreeCoef[i] == (dynamic) 0)
                    {
                        str.Append("");
                    }
                    else
                    {
                        if (i == pairDegreeCoef.Count - 1)
                        {
                            if (pairDegreeCoef[i] == (dynamic) (-1))
                            {
                                str.Append($"-x^{i}");
                            }
                            else if (pairDegreeCoef[i] == (dynamic) 1)
                            {
                                str.Append($"x^{i}");
                            }
                            else
                            {
                                str.Append($"{pairDegreeCoef[i]}x^{i}");
                            }
                        }
                        else
                        {
                            if (pairDegreeCoef[i] > (dynamic) 0)
                            {
                                if (i == 1)
                                {
                                    if (pairDegreeCoef[i] == (dynamic) 1)
                                    {
                                        str.Append($" + x");
                                    }
                                    else
                                    {
                                        str.Append($" + {pairDegreeCoef[i]}x");
                                    }
                                }
                                else
                                {
                                    if (i == 0)
                                    {
                                        str.Append($" + {pairDegreeCoef[i]}");
                                    }
                                    else
                                    {
                                        if (pairDegreeCoef[i] == (dynamic) 1)
                                        {
                                            str.Append($" + x^{i}");
                                        }
                                        else
                                        {
                                            str.Append($" + {pairDegreeCoef[i]}x^{i}");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (pairDegreeCoef[i] == (dynamic) (-1) && i == 1 && i != 0)
                                {
                                    str.Append($" - x");
                                }
                                else
                                {
                                    if (i == 0)
                                    {
                                        str.Append($" - {Math.Abs((dynamic) pairDegreeCoef[i])}");
                                    }
                                    else
                                    {
                                        if (i == 1)
                                        {
                                            str.Append($" - {Math.Abs((dynamic) pairDegreeCoef[i])}x");
                                        }
                                        else
                                        {
                                            if (pairDegreeCoef[i] == (dynamic) (-1))
                                            {
                                                str.Append($" - x^{i}");
                                            }
                                            else
                                            {
                                                str.Append($" - {Math.Abs((dynamic) pairDegreeCoef[i])}x^{i}");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return str.ToString();
        }
    }
}