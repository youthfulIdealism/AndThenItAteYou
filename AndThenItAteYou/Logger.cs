using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survive
{
    public static class Logger
    {
        private static List<string> queuedLogs = new List<string>();
        private static readonly object LogLock = new object();
        private static readonly object FlushLock = new object();
        public static int logTracker { get; private set; }

        public static void flush()
        {
            lock(FlushLock)
            {
                string[] logCopy;
                lock (LogLock)
                {
                    logCopy = queuedLogs.ToArray();
                    queuedLogs.Clear();
                    logTracker = 0;
                }

                if(logCopy.Length > 0)
                {
                    string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Saved Games\\Survive\\Debug\\";
                    string path = directory + "Log.txt";
                    if(!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    if (!File.Exists(path))
                    {
                        File.WriteAllLines(path, logCopy, Encoding.UTF8);
                    }
                    else
                    {
                        using (StreamWriter file = File.AppendText(path))
                        {
                            foreach (string line in logCopy)
                            {
                                file.WriteLine(line);
                            }
                        }
                    }
                }
            }
        }

        public static void log(String logString)
        {
            lock(LogLock)
            {
                logTracker++;
                queuedLogs.Add(logString);
            }
        }

    }
}
