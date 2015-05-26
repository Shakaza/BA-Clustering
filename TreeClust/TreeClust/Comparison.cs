using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeClust
{
    public static class Comparison
    {
        /// <summary>
        /// Compares two DNA sequences
        /// </summary>
        /// <param name="a">First string</param>
        /// <param name="b">Second string</param>
        /// <param name="threshold">Decimal representation of %</param>
        /// <param name="fast"></param>
        /// <returns>True if similarity is higher than the threshold</returns>
        public static bool Compare(string a, string b, double threshold, bool fast = false)
        {
            int result = 0;
            int wrongnr = (int)Math.Ceiling(Math.Min(a.Length, b.Length) * (1 - threshold));
            if (Math.Abs(a.Length - b.Length) > wrongnr)
            {
                return false;
            }
            if (fast)
            {
                result = SpaceEffiFast(a, b, wrongnr);//EditFast(a, b, wrongnr);
            }
            else
            {
                result = EditDistance(a, b);
            }
            return result < wrongnr;
        }

        public static int SpaceEffiFast(string a, string b, int threshold)
        {
            return SpaceEffiFast(a.ToCharArray(), b.ToCharArray(), threshold);
        }

        public static int SpaceEffiFast(char[] a, char[] b, int threshold)
        {
            int[] a1 = new int[b.Length + 1];
            int[] a2 = new int[b.Length + 1];
            for (int i = 0; i < b.Length + 1; i++)
            {
                a1[i] = i;
            }
            int diff = b.Length - a.Length;
            int colmin = 1;
            for (int i = 1; i < a.Length + 1; i++)
            {
                a2[0] = i;
                for (int j = colmin; j < b.Length + 1; j++)
                {
                    if (b[j - 1] == a[i - 1])
                    {
                        a2[j] = a1[j - 1];
                    }
                    else
                    {
                        int min1 = Math.Min(a2[j - 1], a1[j - 1]);
                        a2[j] = Math.Min(min1, a1[j]) + 1;
                    }
                    if (a2[j] > threshold)
                    {
                        int diag = i + diff;
                        if (j < diag)
                        {
                            colmin = j;
                        }
                        else if (j == diag)
                        {
                            return threshold + 1;
                        }
                        else
                        {
                            if (j < b.Length)
                            {
                                a2[j + 1] = threshold + 1;
                            }
                            break;
                        }
                    }
                }
                a2.CopyTo(a1, 0);
                if (colmin != 1 && i < a.Length)
                {
                    a2[colmin] = a1[colmin];
                    colmin++;
                }
            }
            return a1[b.Length];
        }

        public static int EditDistance(string a, string b)
        {
            return EditDistance(a.ToCharArray(), b.ToCharArray());
        }

        private static int EditDistance(char[] a, char[] b)
        {
            int[,] m = new int[b.Length + 1, a.Length + 1];
            for (int i = 0; i < b.Length + 1; i++)
            {
                m[i, 0] = i;
            }
            for (int k = 0; k < a.Length + 1; k++)
            {
                m[0, k] = k;
            }
            for (int i = 1; i < b.Length + 1; i++)
            {
                for (int j = 1; j < a.Length + 1; j++)
                {
                    if (b[i - 1] == a[j - 1])
                    {
                        m[i, j] = m[i - 1, j - 1];
                    }
                    else
                    {
                        int min1 = Math.Min(m[i - 1, j], m[i, j - 1]);
                        m[i, j] = Math.Min(min1, m[i - 1, j - 1]) + 1;
                    }
                }
            }
            return m[b.Length, a.Length];
        }
    }
}
