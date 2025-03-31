using System.Text;
using System.Threading;
using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Game;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Diagnostics.Metrics;


class Message
{
    public string text { get; set; }
    public ConsoleColor color { get; set; }

    public Message(string text, ConsoleColor color)
    {
        this.text = text;
        this.color = color;
    }

}

class bank
{
    public int Id { get; set; }
    public int Money { get; set; }

    public bank(int id, int money)
    {
        Id = id;
        Money = money;
    }
}

namespace Game
{

    class Program
    {
        static List<bank> messages = new List<bank>();
        static readonly object lockObj = new object();
        static Random random = new Random();
        public int i;
        public static void bubble_sort(List<int> array, int len)
        {
            int a;
            bool is_changed = true;
            while (is_changed)
            {
                is_changed = false;
                for (int i = 0; i < len - 1; i++)
                {
                    if (array[i] > array[i + 1])
                    {
                        a = array[i];
                        array[i] = array[i + 1];
                        array[i + 1] = a;
                        is_changed = true;
                    }
                }
            }
        }

        /*public static void CountReversed(int a)
        {
            Thread.Sleep(200);
            for (int i = a; i > 0; i--)
            {
                SendMessage(i.ToString(), ConsoleColor.Red);
                Thread.Sleep(500);
            }
        }*/
        /*public static void Count(int a)
        {
            for (int i = 0; i < a; i++)
            {
                SendMessage(i.ToString(), ConsoleColor.Green);
                Thread.Sleep(500);
            }
        }*/

        public static int Factorial(int a)
        {
            if (a <= 1) return 1;
            return (a * Factorial(a - 1));
        }

        public static void QuickSortWithoutThread(List<int> list)
        {
            if (list.Count < 2)
                return;
            int pivot = list.Last();
            List<int> list_less = new List<int>();
            List<int> list_more = new List<int>();
            List<int> list_equal = new List<int>();
            for (int i = 0; i < list.Count(); i++)
            {
                if (pivot < list[i])
                {
                    list_less.Add(list[i]);
                }
                else if (pivot > list[i])
                {
                    list_more.Add(list[i]);
                }
                else
                {
                    list_equal.Add(list[i]);
                }
            }

            QuickSortWithoutThread(list_less);
            QuickSortWithoutThread(list_more);

            list.Clear();
            list.AddRange(list_less);
            list.AddRange(list_equal);
            list.AddRange(list_more);
        }

        public static void QuickSort(List<int> list)
        {
            if (list.Count < 2)
                return;
            int pivot = list.Last();
            List<int> list_less = new List<int>();
            List<int> list_more = new List<int>();
            List<int> list_equal = new List<int>();
            for (int i = 0; i < list.Count(); i++)
            {
                if (pivot < list[i])
                {
                    list_less.Add(list[i]);
                }
                else if (pivot > list[i])
                {
                    list_more.Add(list[i]);
                }
                else
                {
                    list_equal.Add(list[i]);
                }
            }

            Thread thread1 = new Thread(() => QuickSort(list_less));
            Thread thread2 = new Thread(() => QuickSort(list_more));

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();



            list.Clear();
            list.AddRange(list_less);
            list.AddRange(list_equal);
            list.AddRange(list_more);
        }

        static int sum1;
        static int sum2;

        public static void mySum(List<int> list)
        {
            List<int> list1 = new List<int>();
            List<int> list2 = new List<int>();

            for (int i = 0; i < list.Count() / 2; i++)
            {
                list1.Add(list[i]);
            }
            for (int i = list.Count() / 2; i < list.Count(); i++)
            {
                list2.Add(list[i]);
            }

            Thread thread1 = new Thread(() => sum1 = list1.Sum());
            Thread thread2 = new Thread(() => sum2 = list2.Sum());

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();
            Console.WriteLine();
            Console.WriteLine("Sum: " + (sum1 + sum2));
        }

        /*static void SendMessage(string text, ConsoleColor color)
        {
            messages.Add(new Message(text, color));
        }*/
        
        public static void AddBalance()
        {
            lock(lockObj)
            {
                messages.Add(new bank(Thread.CurrentThread.ManagedThreadId, random.Next(1, 12) * 100));
            }
        }

        public static void RemoveBalance() 
        {
            lock (lockObj)
            {
                messages.Add(new bank(Thread.CurrentThread.ManagedThreadId, random.Next(1, 12) * 100 * -1));
            }
        }

        public static void WhatToDo()
        {
            int a = random.Next(1,3);
            if (a == 1)
                AddBalance();
            else
                RemoveBalance();
            Thread.Sleep(1000);
        }

        static Semaphore semaphore = new Semaphore(3, 3);

        static void ProcessTrain(object trainName)
        {
            
            Console.WriteLine($"[{trainName}] Чекає на вільну колонку");
            semaphore.WaitOne();
            int a = random.Next(3000, 7000);
            Console.WriteLine($"[{trainName}] Заправляється ({a/1000, 0} сек)...");
            Thread.Sleep(a);
            Console.WriteLine($"[{trainName}] Заправку завершено");
            semaphore.Release();
        }

        public static void Main(string[] args)
        {
            Console.OutputEncoding = UTF8Encoding.UTF8;
            Console.InputEncoding = UTF8Encoding.UTF8;
            /*List<Thread> threads = new List<Thread>();
            int balance = 1000;


            for (int i = 0; i < 5; i++)
            {
                threads.Add(new Thread(() => WhatToDo()));
            }

            foreach (var item in threads)
            {
                item.Start();
            }

            while (true)
            {
                foreach (var item in messages.ToArray())
                {
                    if (item.Money >= 0)
                    {
                        balance += item.Money;
                        Console.WriteLine($"{item.Id, 3} | {item.Money, 5} | {balance, 8}");
                    }
                    else if (item.Money < 0 && balance + item.Money >= 0)
                    {
                        balance += item.Money;
                        Console.WriteLine($"{item.Id,3} | {item.Money,5} | {balance,8}");
                    }
                    else
                    {
                        Console.WriteLine($"{item.Id,3} | {item.Money,5} | Недостатньо коштів");
                    }
                }
                Thread.Sleep(5000);
            }*/
            for (int i = 0; i < 10; i++) 
            { 
                Thread trainThread = new Thread(ProcessTrain);
                trainThread.Start($"Авто {i+1, 2}");
            }
        }
    }
}
