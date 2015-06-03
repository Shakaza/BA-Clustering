using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeClust
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Regular clustering:");
                Console.WriteLine("\t -cluster <input file> -id <similarity> -out <output file>");
                Console.WriteLine("Clustering using small memory:");
                Console.WriteLine("\t -cluster_smallmem <input file> -id <similarity> -out <output file>");
                Console.WriteLine("Additional flags:");
                Console.WriteLine("\t -k <kmersize>");
                Console.WriteLine("\t -interval <interval width>");
                Console.WriteLine("\t -buffer <prefetched input size>");
                Console.WriteLine("\t -max <number of sequences to process>");
                return;
            }
            if (args.Length % 2 != 0)
            {
                Console.WriteLine("Error in the number of parameters!");
                return;
            }
            double threshold = 0.97;
            string path = "";
            string output = "";
            int k = 8;
            int interval = 16;
            for (int i = 0; i < args.Length; i += 2)
            {
                string cmd = args[i];
                string value = args[i + 1];
                switch (cmd)
                {
                    case "-id":
                        threshold = double.Parse(value);
                        break;
                    case "-out":
                        output = value;
                        break;
                    case "-cluster":
                        path = value;
                        break;
                    case "-cluster_smallmem":
                        path = value;
                        ClusterStructure.small_mem = true;
                        break;
                    case "-k":
                        k = int.Parse(value);
                        break;
                    case "-interval":
                        interval = int.Parse(value);
                        break;
                    case "-max":
                        FileParser.SetMax(int.Parse(value));
                        break;
                    case "-buffer":
                        FileParser.SetBuffer(int.Parse(value));
                        break;
                    default:
                        Console.WriteLine("Invalid option {0}", cmd);
                        return;
                }
            }
            Output.Path = output;
            if (ClusterStructure.small_mem)
            {
                Kmer.InitializeKmerCounting(k);
            }
            Output.ResetOutput();
            FileInfo file = new FileInfo(path);
            if (file.Extension == ".fna")
            {
                FileParser.fna = true;
            }
            else
            {
                FileParser.fna = false;
            }
            FileParser.fileReader(path);
            ClusterStructure.FixedKMerIntervalTreeSortingCluster(threshold, k, interval);
            Output.queue.CompleteAdding();
            Output.writeTask.Wait();
        }

        private static void CopyNElements(string[] args)
        {
            using (StreamWriter writer = new StreamWriter(new FileStream(args[1], FileMode.OpenOrCreate)))
            {
                StreamReader SR = new StreamReader(new FileStream(args[0], FileMode.Open));
                for (int i = 0; i < int.Parse(args[2]); i++)
                {
                    string sequence = "";
                    char firstChar;
                    string header = SR.ReadLine();
                    while ((firstChar = (char)SR.Read()) != '>')
                    {
                        string line = SR.ReadLine();
                        string replaceWith = "";
                        string removedBreaks = line.Replace("\r\n", replaceWith).Replace("\n", replaceWith).Replace("\r", replaceWith);
                        sequence = sequence + firstChar + removedBreaks;
                    }
                    writer.WriteLine(">" + header);
                    writer.WriteLine(sequence);
                    Console.Write("\rWrote {0}/{1}", i + 1, int.Parse(args[2]));
                }
                SR.Close();
            }
        }
    }
}
