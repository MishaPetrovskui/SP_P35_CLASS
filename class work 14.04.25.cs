using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SmartBackup
{
    enum Directory
    {
        reserv,
        nonReserv
    }
    class backupDirectory
    {
        static string BackupDirectory {  get; set; }
        static string path {  get; set; }
        static Directory Directory { get; set; }
    }

    class exam
    {
        static List<string> save_list = new List<string>();
        static string backupDirectory = "C:\\exam_backup";
        static Random rnd = new Random();
        

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CopyFile(string lpFileName, string lpNewFileName, bool overwrite);

        static bool stopRequested = false;

        public static void ShowProcessesWithInterval(int seconds)
        {
            stopRequested = false;

            Thread worker = new Thread(() =>
            {
                while (!stopRequested)
                {
                    Console.Clear();
                    Console.WriteLine("Список процесів (натисніть Enter для виходу):\n");
                    foreach (var process in save_list)
                    {
                        Console.WriteLine($"{process,-30} to {backupDirectory}");
                    }

                    for (int i = 0; i < seconds * 10; i++)
                    {
                        if (stopRequested) return;
                        Thread.Sleep(100);
                    }
                }
            });

            worker.Start();

            Console.ReadLine();
            stopRequested = true;
            worker.Join();
        }

        public static void AddProcess(string input, string bd)
        {
            int pid;
            if (true)
            {
                try
                {
                    string fileName = Path.GetFileName(input);
                    bool _ = true;
                    foreach (var process in save_list)
                    {
                        if (process == null || process != input)
                        {
                            save_list.Add(input);
                            _ = false;
                            Console.WriteLine(_);
                        }
                    }
                    bool result = CopyFile(input, $"{bd}\\{fileName}", _);
                    Console.WriteLine("Added to \"C:\\exam_backup\"");

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Помилка: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Невірний шлях.");
            }
        }

        public static void KillProcess()
        {
            Console.Write("Введіть ID процесу для завершення: ");
            string input = Console.ReadLine();
            int pid;
            if (int.TryParse(input, out pid))
            {
                try
                {
                    Process.GetProcessById(pid).Kill();
                    Console.WriteLine("Процес завершено.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Помилка: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Невірний ID.");
            }
        }

        public static void StartApplication()
        {
            Console.WriteLine("Оберіть програму:");
            Console.WriteLine("1. Notepad");
            Console.WriteLine("2. Calculator");
            Console.WriteLine("3. Paint");
            Console.WriteLine("4. Власна програма");
            Console.Write("Ваш вибір: ");
            string input = Console.ReadLine();

            try
            {
                switch (input)
                {
                    case "1":
                        Process.Start("notepad.exe");
                        break;
                    case "2":
                        Process.Start("calc.exe");
                        break;
                    case "3":
                        Process.Start("mspaint.exe");
                        break;
                    case "4":
                        Console.Write("Введіть повний шлях: ");
                        string path = Console.ReadLine();
                        Process.Start(path);
                        break;
                    default:
                        Console.WriteLine("Невірний вибір.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка запуску: " + ex.Message);
            }
        }


        static void Main(string[] args)
        {
            Console.InputEncoding = UTF8Encoding.UTF8;
            Console.OutputEncoding = UTF8Encoding.UTF8;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== МЕНЮ ===");
                Console.WriteLine("1. Додати");
                Console.WriteLine("2. Переглянути");
                Console.WriteLine("3. Налаштувати");
                Console.WriteLine("4. Видалити");
                Console.WriteLine("5. Вийти");
                Console.Write("\nВаш вибір: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Введіть шлях до процесу: ");
                        string input = Console.ReadLine();
                        Console.Write("Введіть шлях куди копіювати: ");
                        string backupDirectory = Console.ReadLine();
                        AddProcess(input, backupDirectory);
                        break;

                    case "2":
                        Console.Write("Введіть інтервал оновлення (сек): ");
                        if (int.TryParse(Console.ReadLine(), out int interval))
                            ShowProcessesWithInterval(interval);
                        else
                            Console.WriteLine("Невірний інтервал.");
                        break;

                    case "3":
                        KillProcess();
                        break;

                    case "4":
                        StartApplication();
                        break;

                    case "5":
                        return;

                    default:
                        Console.WriteLine("Невірна команда.");
                        break;
                }

                Console.WriteLine("\nНатисніть Enter для продовження...");
                Console.ReadLine();
            }
        }
    }
}
