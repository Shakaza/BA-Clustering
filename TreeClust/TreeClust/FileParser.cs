using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeClust
{
    class FileParser
    {
        private static StreamReader SR;
        public static bool fna;
        public static Task t;
        public static int Size = 10;
        public static string path;
        public static int Read = 0;
        private static Queue<string> Q;
        public static bool forceStop = false;
        private static int MaxIter = int.MaxValue;
        public static void fileReader(string path)
        {
            FileParser.path = path;
            Q = new Queue<string>();
            SR = new StreamReader(new FileStream(path, FileMode.Open));
            for (int i = 0; i < Size; i++)
            {
                string sequence = "";
                if (fna) 
                {
                    char firstChar;
                    string header = SR.ReadLine();
                    while((firstChar = (char)SR.Read()) != '>')
                    {
                        string line = SR.ReadLine();
                        string replaceWith = "";
                        string removedBreaks = line.Replace("\r\n", replaceWith).Replace("\n", replaceWith).Replace("\r", replaceWith);
                        sequence = sequence + firstChar + removedBreaks;
                    }
                    
                }
                else
                {
                    sequence = SR.ReadLine();
                }
                Q.Enqueue(sequence);
            }
            forceStop = false;
            Read = Size;
            t = new Task(loop);
            t.Start();
        }

        public static void SetBuffer(int size)
        {
            Size = size;
        }

        private static void loop()
        {
            while (!SR.EndOfStream && !forceStop && Read < MaxIter)
            {
                while (Q.Count >= Size) ;
                string sequence = "";
                if (fna)
                {
                    char firstChar;
                    string header = SR.ReadLine();
                    while (!SR.EndOfStream && (firstChar = (char)SR.Read()) != '>')
                    {
                        string line = SR.ReadLine();
                        string replaceWith = "";
                        string removedBreaks = line.Replace("\r\n", replaceWith).Replace("\n", replaceWith).Replace("\r", replaceWith);
                        sequence = sequence + firstChar + removedBreaks;
                    }

                }
                else
                {
                    sequence = SR.ReadLine();
                }
                Q.Enqueue(sequence);
                Read++;
            }
            SR.Close();
        }

        public static void SetMax(int max)
        {
            MaxIter = max;
        }

        public static void Stop()
        {
            forceStop = true;
        }

        public static string getNext()
        {
            while (Q.Count == 0 && t.Status == TaskStatus.Running && Read < MaxIter) ;
            try
            {
                string seq = Q.Dequeue();
                if (seq != null)
                {
                    seq = seq.ToLower();
                }
                return seq;
            }
            catch
            {
                return null;
            }
        }
    }
}
