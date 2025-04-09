using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SP_P35
{
    class ConsoleWindow
    {
        object LockConsole;
        object LockMessages;
        Point pos;
        int width;
        int height;
        public ConsoleColor ForegroundColor;
        List<string> text = new List<string>();

        public ConsoleWindow(Point pos, int width, int height, ConsoleColor ForegroundColor, object lockconsole, object lockmessages)
        {
            this.pos = pos;
            this.width = width;
            this.height = height;
            this.ForegroundColor = ForegroundColor;
            this.LockConsole = lockconsole;
            this.LockMessages = lockmessages;
        }

        public void DrawWindow()
        {
            for (int j = 0; j < width; j++)
            {
                Console.SetCursorPosition(pos.Y+j, pos.X);
                Console.BackgroundColor = ForegroundColor;
                Console.Write(" ");
                Console.ResetColor();
            }
            for (int j = 0; j < height; j++)
            {
                Console.SetCursorPosition(pos.Y, pos.X + j);
                Console.BackgroundColor = ForegroundColor;
                Console.Write(" ");
                Console.ResetColor();
            }
            for (int j = 0; j < height; j++)
            {
                Console.SetCursorPosition(pos.Y + width, pos.X + j);
                Console.BackgroundColor = ForegroundColor;
                Console.Write(" ");
                Console.ResetColor();
            }
            for (int j = 0; j < width + 1; j++)
            {
                Console.SetCursorPosition(pos.Y + j, pos.X + height);
                Console.BackgroundColor = ForegroundColor;
                Console.Write(" ");
                Console.ResetColor();
            }
        }

        public void WriteLine(string text)
        {
            lock (LockMessages)
            {
                if (text.Count() > height - pos.Y)
                    this.text.Remove(this.text[0]);
                else
                {
                    this.text.Add(text + "\n");
                }
            }
        }

        public void Write(string text)
        {
            lock (LockMessages)
            {
                if (text.Count() > height - pos.Y)
                    this.text.Remove(this.text[0]);
                while (text.Length > width - pos.X)
                {
                    string a = "";
                    for (int i = 0; i < width - pos.X; i++)
                    {
                        a += text[i];
                        text.Remove(i);
                    }
                    this.text.Add(a);
                }
                if (this.text.Last().Length + text.Length > width - pos.X)
                {
                    this.text[text.Count() - 1] = this.text.Last() + text;
                }
                else
                    this.text.Add(text);
            }
        }

        public void Draw()
        {
            lock (LockConsole)
            {
                lock (LockMessages)
                {
                    int j = 0;
                    int i = 0;
                    foreach (var lll in text)
                    {
                        for (int k = 0; k < lll.Length; k++) 
                        {
                            if (j >= width)
                            {
                                j = 0;
                                i++;
                            }
                            if (lll[k] == '\n')
                            {
                                j = 0;
                                i++;
                            }
                            /*if (j >= height)
                                text.Last().Remove(lll[k]);*/
                            else
                            {
                                Console.SetCursorPosition(pos.X + j++, pos.Y + i);
                                Console.Write(lll[k]);
                            }
                        }
                    }
                }
            }
        }
    }
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

                    int i = 0;
                    foreach (var lll in messages)
                    {
                        Console.SetCursorPosition(42, i++);
                        Console.Write(lll);
                    }
                }
        }

        public static void AddMessage(string message)
        {
            lock (LockMessages)
            {
                if (messages.Count() < 20)
                    messages.Add(message);
                else
                {
                    messages.Remove(messages[0]); Console.Clear();
                }
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
                        return active;
                    }
                }
                Thread.Sleep(100);
            }
        }

        static Semaphore semaphore = new Semaphore(3, 3);

        static void ProcessTrain(int num)
        {
            semaphore.WaitOne();
            AddMessage($"[N{num + 1}] Чекає на вільну колонку");
            int a = random.Next(3000, 7000);
            AddMessage($"[N{num + 1}] Заправляється ({a / 1000,0} сек)...");
            Thread.Sleep(a);
            AddMessage($"[N {num + 1}] Заправку завершено");
            semaphore.Release();
        }

        public static void start(int num)
        {
            Parallel.For(0, num, i => { ProcessTrain(i); });
        }

        public static void Main(string[] args)
        {
            Console.OutputEncoding = UTF8Encoding.UTF8;
            Console.InputEncoding = UTF8Encoding.UTF8;
            IntPtr consoleWindow = GetConsoleWindow();
            ShowWindow(consoleWindow, 3);
            Console.CursorVisible = false;
            List<ConsoleWindow> actions = new List<ConsoleWindow> { new ConsoleWindow(new Point(0, 0), 30, 15, ConsoleColor.Blue, LockConsole, LockMessages), new ConsoleWindow(new Point(0, 42), 30, 15, ConsoleColor.Blue, LockConsole, LockMessages) };
            
            int num = 1;
            Parallel.Invoke(
                () =>
                {
                    actions[0].DrawWindow();
                    while (true)
                    {
                        actions[0].Draw();
                    }
                },
                () =>
                {
                    actions[0].DrawWindow();
                    while (true)
                    {
                        actions[1].Draw();
                    }
                },
                () =>
                {
                    while (true)
                    {
                        ConsoleKey key = Console.ReadKey(true).Key;
                        if (key == ConsoleKey.C)
                        {
                            
                        }
                    }
                }
            );




            Console.SetCursorPosition(1, 23);
            /*List<string> actions = new List<string> { "Print \"Hello!\"", "Add 1 car", "Add 5 cars", "Add 10 cars", "Exit" };
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Clear();
            int num = 1;
            Parallel.Invoke(
                () =>
                {
                    while (true)
                    {
                        switch (Menu(actions))
                        {
                            case 0: { AddMessage("Hello!"); break; }
                            case 1:
                                {
                                    num++; Thread trainThread = new Thread(() => start(num));
                                    trainThread.Start(); break;
                                }
                            case 2:
                                {
                                    num = num + 5; Thread trainThread = new Thread(() => start(num));
                                    trainThread.Start(); break;
                                }
                            case 3:
                                {
                                    num = num + 10; Thread trainThread = new Thread(() => start(num));
                                    trainThread.Start(); break;
                                }
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
            );*/
        }
    }
}
