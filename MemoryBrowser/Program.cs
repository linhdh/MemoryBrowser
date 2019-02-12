using System;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

namespace MemoryBrowser
{
    class Program
    {
        private const float KB = 1024;
        private const float MB = 1048576;
        private const float GB = 1073741824;
        private const float TB = 1099511627776;
        private const string COMMAND_ERROR = "Command had error!";
        private const string UNSUPPORTED_COMMAND = "Unsupported Command!";
        private static bool _iWantToExit = false;
        static void Main(string[] args)
        {
            string line = Console.ReadLine();
            while(!string.IsNullOrWhiteSpace(line))
            {
                string[] parts = line.Split(' ');
                switch(parts[0].ToLower())
                {
                    case "list":
                        ListAllRunningProcesses();
                        break;
                    case "detail":
                        if (parts.Length == 2)
                        {
                            try
                            {
                                int pid = int.Parse(parts[1]);
                                var processes = Process.GetProcesses();
                                var process = processes.FirstOrDefault(p => p.Id == pid);
                                if (process != null)
                                {
                                    PrintDetailProcessInfo(process);
                                }
                                else
                                {
                                    throw new Exception("Process was not found!");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        else
                        {
                            Console.WriteLine(COMMAND_ERROR);
                        }
                        break;
                    case "help":
                        break;
                    case "exit":
                        _iWantToExit = true;
                        break;
                    default:
                        Console.WriteLine(UNSUPPORTED_COMMAND);
                        break;
                }
                if (_iWantToExit)
                {
                    break;
                }
                line = Console.ReadLine();
            }
            Console.WriteLine("Press any key to exit!");
            Console.ReadKey();
        }

        private static void ListAllRunningProcesses()
        {
            var processes = Process.GetProcesses();
            string linePattern = "{0,-10}{1,-50}{2,-18:F3}{3,-15:F3}{4,-15:F3}{5,-10}";
            Console.WriteLine(string.Format(linePattern, "PID", "Name", "Physical Memory", "Private Memory", "Virtual Memory", "Handle"));
            foreach (Process process in processes)
            {
                Console.WriteLine(string.Format(linePattern, process.Id, process.ProcessName, NumberToMemoryString(process.WorkingSet64), NumberToMemoryString(process.PrivateMemorySize64),
                    NumberToMemoryString(process.VirtualMemorySize64), process.HandleCount));
            }
            Console.WriteLine(processes.Length + " process(es) are found.");
        }

        private static void PrintDetailProcessInfo(Process process)
        {
            Console.WriteLine();
            string pattern = "{0}: {1}";
            Console.WriteLine(string.Format(pattern, "Name", process.ProcessName));
            Console.WriteLine(string.Format(pattern, "Id", process.Id));
            Console.WriteLine(string.Format(pattern, "SessionId", process.SessionId));
            Console.WriteLine(string.Format(pattern, "BasePriority", process.BasePriority));
            Console.WriteLine(string.Format(pattern, "Handle", process.Handle));
            Console.WriteLine(string.Format(pattern, "HandleCount", process.HandleCount));
            Console.WriteLine(string.Format(pattern, "MainWindowHandle", process.MainWindowHandle));
            Console.WriteLine(string.Format(pattern, "MainWindowTitle", process.MainWindowTitle));
            Console.WriteLine(string.Format(pattern, "MinWorkingSet", NumberToMemoryString((long)process.MinWorkingSet)));
            Console.WriteLine(string.Format(pattern, "MaxWorkingSet", NumberToMemoryString((long)process.MaxWorkingSet)));
            Console.WriteLine(string.Format(pattern, "NonpagedSystemMemorySize", NumberToMemoryString(process.NonpagedSystemMemorySize64)));
            Console.WriteLine(string.Format(pattern, "PagedMemorySize", NumberToMemoryString(process.PagedMemorySize64)));
            Console.WriteLine(string.Format(pattern, "PagedSystemMemorySize", NumberToMemoryString(process.PagedSystemMemorySize64)));
            Console.WriteLine(string.Format(pattern, "WorkingSet", NumberToMemoryString(process.WorkingSet64)));
            Console.WriteLine(string.Format(pattern, "VirtualMemorySize", NumberToMemoryString(process.VirtualMemorySize64)));
            Console.WriteLine(string.Format(pattern, "PeakWorkingSet", NumberToMemoryString(process.PeakWorkingSet64)));
            Console.WriteLine(string.Format(pattern, "PeakVirtualMemorySize", NumberToMemoryString(process.PeakVirtualMemorySize64)));
            Console.WriteLine(string.Format(pattern, "PeakPagedMemorySize", NumberToMemoryString(process.PeakPagedMemorySize64)));
            Console.WriteLine(string.Format(pattern, "PrivateMemorySize", NumberToMemoryString(process.PrivateMemorySize64)));
            Console.WriteLine(string.Format(pattern, "UserProcessorTime", process.UserProcessorTime));
            Console.WriteLine(string.Format(pattern, "TotalProcessorTime", process.TotalProcessorTime));
            Console.WriteLine(string.Format(pattern, "Threads", process.Threads.Count));
            Console.WriteLine(string.Format(pattern, "StartTime", process.StartTime));
            Console.WriteLine(string.Format(pattern, "ProcessorAffinity", process.ProcessorAffinity));
            Console.WriteLine(string.Format(pattern, "PrivilegedProcessorTime", process.PrivilegedProcessorTime));
            if (process.HasExited)
            {
                Console.WriteLine(string.Format(pattern, "ExitCode", process.ExitCode));
                Console.WriteLine(string.Format(pattern, "ExitTime", process.ExitTime));
            }
            Console.WriteLine();
            //var properties = typeof(Process).GetProperties();
            //foreach (var property in properties)
            //{
            //    switch (property.PropertyType.FullName.ToLower())
            //    {
            //        case "system.int32":
            //        case "system.int64":
            //        case "system.intptr":
            //            if (property.Name == "ExitCode")
            //                break;
            //            Console.WriteLine(string.Format("{0}: {1}", property.Name, property.GetValue(process)));
            //            break;
            //        case "boolean":
            //            break;
            //        case "system.datetime":
            //            break;
            //        case "system.string":
            //            break;
            //        case "system.timespan":
            //            break;
            //        default:
            //            break;
            //    }
            //}
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
