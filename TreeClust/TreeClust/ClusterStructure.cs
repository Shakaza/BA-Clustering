using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TreeClust
{
    static class ClusterStructure
    {
        internal static bool small_mem = false;
        public static Node n; 
        internal static int i = 0;
        internal static int Clustered = 0;

        public static Node FixedKMerIntervalTreeSortingCluster(double treshold, int k, int intervalLength)
        {
            Thread console = new Thread(ConsoleOutput);
            console.Start();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            string seq;
            seq = FileParser.getNext();
            if (!small_mem)
            {
                n = new Node(i, seq, Kmer.KmerCounting(seq, k));
            }
            else
            {
                n = new Node(i, seq, Kmer.KmerCountingI(seq, k));
            }
            i = 1;
            seq = FileParser.getNext();
            while (seq != null)
            {
                int lower = (int)Math.Floor((1 - treshold) * seq.Length);
                if (!small_mem)
                {
                    n.Insert(Kmer.KmerCounting(seq, k), seq, i, lower, lower * k, treshold, intervalLength);
                }
                else
                {
                    n.Insert(Kmer.KmerCountingI(seq, k), seq, i, lower, lower * k, treshold, intervalLength);
                }
                seq = FileParser.getNext();
                i++;
            }
            watch.Stop();
            console.Abort();
            Console.Write("\rClustered {0:n0} Clusters: {1:n0} s/sek: {3:n1} Time: {2}  ", i, i - Clustered, watch.Elapsed.ToString(@"hh\:mm\:ss"), Math.Round(i / (watch.ElapsedMilliseconds / 1000.0d), 1));
            Console.WriteLine();
            Console.WriteLine("Clustering took " + watch.Elapsed.ToString());
            return n;
        }

        static void ConsoleOutput()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (true)
            {
                try
                {
                    Thread.Sleep(100);
                    Console.Write("\rClustered {0:n0} Clusters: {1:n0} s/sek: {3:n1} Time: {2} Ram: {4:n0}mb  ", i, i - Clustered, watch.Elapsed.ToString(@"hh\:mm\:ss"), Math.Round(i / (watch.ElapsedMilliseconds / 1000.0d), 1), GC.GetTotalMemory(true) / (1024 * 1024));
                }
                catch
                {

                }
            }
        }
    }
}
