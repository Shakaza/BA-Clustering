using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeClust
{
    class Output
    {
        public static string Path = "C:\\ClusterOutput.txt";
        public static Task writeTask;
        public static BlockingCollection<string> queue = new BlockingCollection<string>();
        public static void WriteOutputLine(int sequenceId, int ClusterId, char type = 'M', string comment = null)
        {
            string text = sequenceId + "   :   " + type + "   :   " + ClusterId;
            if (comment != null)
            {
                text += "       " + comment;
            }
            queue.Add(text);
        }

        private static void WriteToFile()
        {
            while (!queue.IsAddingCompleted)
            {
                while (queue.Count == 0) ;
                using (StreamWriter stream = File.AppendText(Path))
                {
                    foreach (string s in queue.GetConsumingEnumerable())
                    {
                        stream.WriteLine(s);
                    }
                }
            }
        }

        public static void WriteLine(string text)
        {
            using (StreamWriter SW = File.AppendText(Path))
            {
                SW.WriteLine(text);
            }
        }

        public static void Write(string text)
        {
            using (StreamWriter SW = File.AppendText(Path))
            {
                SW.Write(text);
            }
        }

        public static void ResetOutput()
        {
            File.Delete(Path);
            writeTask = new Task(WriteToFile);
            writeTask.Start();
        }
    }
}
