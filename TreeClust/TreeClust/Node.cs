using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeClust
{
    class Node
    {
        public int[] KmerI { get; set; }
        public long[] KmerL { get; set; }
        public string Centroid { get; set; }
        public Dictionary<int, Node> Children { get; set; }
        
        private static int NodeCount = 0;

        public int NodeId { get; private set; }

        public Node(int seqid, string Cent, int[] Kmers)
        {
            this.NodeId = NodeCount;
            NodeCount++;
            this.Centroid = Cent;
            this.KmerI = Kmers;
            Children = new Dictionary<int, Node>();
            Output.WriteOutputLine(seqid, NodeId, 'C');
        }

        public Node(int seqid, string Cent, long[] Kmers)
        {
            this.NodeId = NodeCount;
            NodeCount++;
            this.Centroid = Cent;
            this.KmerL = Kmers;
            Children = new Dictionary<int, Node>();
            Output.WriteOutputLine(seqid, NodeId, 'C');
        }

        public void Insert(long[] kmers, string seq, int seqid, int lower, int upper, double Threshold, int intervalLength = 16)
        {
            int kmerDiff = Kmer.calcCountInt(kmers, KmerL);
            if (kmerDiff <= lower)
            {
                Output.WriteOutputLine(seqid, this.NodeId);
                ClusterStructure.Clustered++;
                return;
            }
            if (kmerDiff <= upper)
            {
                if (Comparison.Compare(seq, this.Centroid, 0.97, true))
                {
                    Output.WriteOutputLine(seqid, this.NodeId);
                    ClusterStructure.Clustered++;
                    return;
                }
            }
            int intervalStart = kmerDiff / intervalLength;
            if (Children.ContainsKey(intervalStart))
            {
                Children[intervalStart].Insert(kmers, seq, seqid, lower, upper, Threshold, intervalLength);
            }
            else
            {
                Children[intervalStart] = new Node(seqid, seq, kmers);
            }
        }

        public void Insert(int[] kmers, string seq, int seqid, int lower, int upper, double Threshold, int intervalLength = 16)
        {
            int kmerDiff = Kmer.calcCountInt(kmers, KmerI);
            if (kmerDiff <= lower)
            {
                Output.WriteOutputLine(seqid, this.NodeId);
                ClusterStructure.Clustered++;
                return;
            }
            if (kmerDiff <= upper)
            {
                if (Comparison.Compare(seq, this.Centroid, 0.97, true))
                {
                    Output.WriteOutputLine(seqid, this.NodeId);
                    ClusterStructure.Clustered++;
                    return;
                }
            }
            int intervalStart = kmerDiff / intervalLength;
            if (Children.ContainsKey(intervalStart))
            {
                Children[intervalStart].Insert(kmers, seq, seqid, lower, upper, Threshold, intervalLength);
            }
            else
            {
                Children[intervalStart] = new Node(seqid, seq, kmers);
            }
        }
    }
}
