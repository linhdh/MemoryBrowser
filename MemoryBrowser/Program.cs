using System;
using System.Diagnostics;

namespace MemoryBrowser
{
    class Program
    {
        private const float KB = 1024;
        private const float MB = 1048576;
        private const float GB = 1073741824;
        private const float TB = 1099511627776;
        static void Main(string[] args)
        {
            var processes = Process.GetProcesses();
            string linePattern = "{0,-10}{1,-50}{2,-18:F3}{3,-15:F3}{4,-15:F3}{5,-10}";
            Console.WriteLine(processes.Length + " process(es) are found.");
            Console.WriteLine(string.Format(linePattern, "PID", "Name", "Physical Memory", "Private Memory", "Virtual Memory", "Handle"));
            foreach (Process process in processes)
            {
                Console.WriteLine(string.Format(linePattern, process.Id, process.ProcessName, NumberToMemoryString(process.WorkingSet64), NumberToMemoryString(process.PrivateMemorySize64), 
                    NumberToMemoryString(process.VirtualMemorySize64), process.HandleCount));
            }
            Console.WriteLine("Press any key to exit!");
            Console.ReadKey();
        }

        private static string NumberToMemoryString(long bytes)
        {
            string pattern = "{0} {1}";
            string result = string.Empty;
            if (bytes < KB)
            {
                result = string.Format(pattern, bytes, "byte(s)");
            }
            else if (bytes < MB)
            {
                result = string.Format(pattern, bytes / KB, "KB");
            }
            else if (bytes < GB)
            {
                result = string.Format(pattern, bytes / MB, "MB");
            }
            else if (bytes < TB)
            {
                result = string.Format(pattern, bytes / GB, "GB");
            }
            else
            {
                result = string.Format(pattern, bytes / TB, "TB");
            }
            return result;
        }
    }
}
