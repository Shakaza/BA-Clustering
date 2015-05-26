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
        public static bool skipLine;
        public static Task t;
        public static int Size = 10;
        public static string path;
        private static int Read = 0;
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
                if (skipLine) SR.ReadLine();
                Q.Enqueue(SR.ReadLine());
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
                if (skipLine) SR.ReadLine();
                Q.Enqueue(SR.ReadLine());
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
