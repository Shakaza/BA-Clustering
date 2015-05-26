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
            FileParser.fileReader(path);
            if (file.Extension == "fna")
            {
                FileParser.skipLine = true;
            }
            else
            {
                FileParser.skipLine = false;
            }
            ClusterStructure.FixedKMerIntervalTreeSortingCluster(threshold, k, interval);
        }
    }
}
