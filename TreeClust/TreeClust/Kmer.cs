using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeClust
{
    static class Kmer
    {
        public static int[] Multipliers;

        public static void InitializeKmerCounting(int k)
        {
            Multipliers = new int[k];
            Multipliers[0] = 1;
            for (int j = 1; j < k; j++)
            {
                Multipliers[j] = Multipliers[j - 1] << 2;
            }
        }

        public static int calcCountInt(long[] a1, long[] a2)
        {
            int count = 0;
            int i = 0;
            int h = 0;
            while (i < a1.Length && h < a2.Length)
            {
                if (a1[i] == a2[h])
                {
                    i++;
                    h++;
                }
                else if (a1[i] < a2[h])
                {
                    i++;
                    count++;
                }
                else
                {
                    h++;
                    count++;
                }
            }
            if (i < a1.Length)
            {
                count += (a1.Length - i);
            }
            if (h < a2.Length)
            {
                count += (a2.Length - h);
            }
            return count;
        }

        public static int calcCountInt(int[] a1, int[] a2)
        {
            int count = 0;
            int i = 0;
            int h = 0;
            while (i < a1.Length && h < a2.Length)
            {
                if (a1[i] == a2[h])
                {
                    i++;
                    h++;
                }
                else if (a1[i] < a2[h])
                {
                    i++;
                    count++;
                }
                else
                {
                    h++;
                    count++;
                }
            }
            if (i < a1.Length)
            {
                count += (a1.Length - i);
            }
            if (h < a2.Length)
            {
                count += (a2.Length - h);
            }
            return count;
        }

        public static long[] KmerCounting(string sequence, int k)
        {
            long[] tmp = new long[sequence.Length + 1 - k];
            for (int i = 0; i < tmp.Length; i++)
            {
                string kmer = sequence.Substring(i, k);
                long h = toint(kmer, k);
                tmp[i] = h;
            }
            Array.Sort(tmp);
            return tmp;
        }

        public static int[] KmerCountingI(string sequence, int k)
        {
            int[] tmp = new int[sequence.Length + 1 - k];
            for (int i = 0; i < tmp.Length; i++)
            {
                string kmer = sequence.Substring(i, k);
                int h = (int)toint(kmer, k);
                tmp[i] = h;
            }
            Array.Sort(tmp);
            return tmp;
        }

        public static long toint(string seq, int k)
        {
            if (ClusterStructure.small_mem)
            {
                int h = 0;
                for (int i = 1; i <= k; i++)
                {
                    int front = basetoint(seq[k - i]);
                    h = h + Multipliers[i - 1] * front;
                }
                return h;
            }
            else
            {
                long h = 0;
                byte[] array = new byte[8];
                byte[] array1 = Encoding.ASCII.GetBytes(seq);
                if (array1.Length < 8)
                {
                    array1.CopyTo(array, 8 - array1.Length);
                }
                else
                {
                    array = array1;
                }
                h = BitConverter.ToInt64(array, 0);
                return h;
            }
        }

        private static int basetoint(char c)
        {
            switch (c)
            {
                case 'a':
                    return 0;
                case 'c':
                    return 1;
                case 'g':
                    return 2;
                case 't':
                    return 3;
            }
            return 0;
        }
    }
}
