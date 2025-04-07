using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SP_P35
{
    internal class MultiWindows
    {
        static object LockConsole = new object();
        static object LockMessages = new object();
        public static Random random = new Random();
        public static List<string> messages = new List<string>();
        public static void Count()
        {
            lock (LockConsole)
            {
                Console.SetCursorPosition(40, 0);
                Console.WriteLine(random.Next(0, 10));
            }
        }

        public static void OutputWindow()
        {
            lock (LockConsole)
                lock (LockMessages)
                {
                    for (int i = 0; i < messages.Count; i++)
                    {
                        Console.SetCursorPosition(42, i);
                        Console.Write(messages[i]);
                    }
                }
        }

        public static void AddMessage(string message)
        {
            lock (LockMessages)
            {
                if (messages.Count < 10)
                    messages.Add(message);
            }
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static int Menu(IEnumerable<string> item)
        {
            int active = 0;
            while (true)
            {
                lock (LockConsole)
                {
                    Console.SetCursorPosition(0, 0);
                    for (int i = 0; i < item.Count(); i++)
                    {
                        if (i == active) Console.WriteLine($" > {item.ElementAt(i)}");
                        else Console.WriteLine($"   {item.ElementAt(i)}");

                    }
                }
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.UpArrow) active = Math.Max(active - 1, 0);
                    else if (key == ConsoleKey.DownArrow) active = Math.Min(active + 1, item.Count() - 1);
                    else if (key == ConsoleKey.Enter)
                    {
                        Console.Clear(); return active;
                    }
                }
                Thread.Sleep(100);
            }
        }



        public static void Main(string[] args)
        {
            IntPtr consoleWindow = GetConsoleWindow();
            ShowWindow(consoleWindow, 3);
            Console.CursorVisible = false;
            List<string> actions = new List<string> { "Print \"Hello!\"", "Action 1", "Action 2", "Action 3", "Exit" };
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();

            Parallel.Invoke(
                () =>
                {
                    while (true)
                    {
                        switch (Menu(actions))
                        {
                            case 0: { AddMessage("Hello!"); break; }
                            case 1: { Menu(new string[] { "Back", "Another back" }); break; }
                            case 2: { Menu(new string[] { "Back", "Another back" }); break; }
                            case 3: { Menu(new string[] { "Back", "Another back" }); break; }
                            case 4: { Environment.Exit(0); break; }
                        }
                    }
                },
                () =>
                {
                    while (true) Count();
                },
                () =>
                {
                    while (true) OutputWindow();
                }
            );
        }
    }
}
