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
    public int HowMany { get; set; }

    public bank(int id, int howmany)
    {
        Id = id;
        HowMany = howmany;
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

        /*public static void AddBalance()
        {
            lock (lockObj)
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
            int a = random.Next(1, 3);
            if (a == 1)
                AddBalance();
            else
                RemoveBalance();
            Thread.Sleep(1000);
        }*/

        static SemaphoreSlim semaphore = new SemaphoreSlim(20, 20);

        static void Process(object trainName)
        {
            semaphore.Wait();
            Console.WriteLine($"[{trainName}] Чекає на вхід");
            int a = random.Next(3000, 7000);
            Console.WriteLine($"[{trainName}] Зайшов на стадіон (чекати {a / 1000,0} секунд)...");
            Thread.Sleep(a);
            Console.WriteLine($"[{trainName}] Вийшов з стадіону");
            semaphore.Release();
        }

        static void MultiplicationTable(int num)
        {
            Parallel.For(0, num + 1, i =>
            {
                Console.WriteLine($"[{num}] {num} × {i} = {num * i}");
            });
        }

        static int sum(List<int> list)
        {
            int sum = 0;
            for (int i = 0; i < list.Count(); i++)
                sum += list[i];
            return sum;
        }

        static double avg(List<int> list)
        {
            double sum = 0;
            for (int i = 0; i < list.Count(); i++)
                sum += list[i];
            return sum / list.Count();
        }

        static int min(List<int> list)
        {
            int min = list[0];
            for (int i = 0; i < list.Count();i++)
                if (list[i] < min)
                    min = list[i];
            return min;
        }

        static int max(List<int> list)
        {
            int max = list[0];
            for (int i = 0; i < list.Count(); i++)
                if (list[i] > max)
                    max = list[i];
            return max;
        }

        public static void Main(string[] args)
        {
            Console.OutputEncoding = UTF8Encoding.UTF8;
            Console.InputEncoding = UTF8Encoding.UTF8;

            /*Thread _ = new Thread(() => { while (true) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"залишилось {semaphore.CurrentCount} місць"); Console.ResetColor(); Thread.Sleep(10000); } });
            _.Start();

            for (int i = 0; i < 1025; i++)
            {
                thread trainthread = new thread(process);
                trainthread.start($"людина {i + 1,2}");
                thread.sleep(1000);
            }*/

            //Parallel.For(0, 11, i =>
            //{
            //    MultiplicationTable(i);
            //    Thread.Sleep(200);
            //});

            List<int> list = new List<int>();

            for(int i = 0;i < 100000000;i++)
                list.Add(random.Next(0,100000000));
            Stopwatch stopwatch1 = Stopwatch.StartNew();
            Parallel.Invoke(
                () =>
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    var _ = sum(list);
                    stopwatch.Stop();
                    Console.WriteLine($"sum = {_} for {stopwatch.ElapsedMilliseconds} ms");
                },
                () =>
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    var _ = avg(list);
                    stopwatch.Stop();
                    Console.WriteLine($"avg = {_} for {stopwatch.ElapsedMilliseconds} ms");
                },
                () =>
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    var _ = min(list);
                    stopwatch.Stop();
                    Console.WriteLine($"min = {_} for {stopwatch.ElapsedMilliseconds} ms");
                },
                () =>
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    var _ = max(list);
                    stopwatch.Stop();
                    Console.WriteLine($"max = {_} for {stopwatch.ElapsedMilliseconds} ms");
                }
            );
            stopwatch1.Stop();
            Console.WriteLine($"full for {stopwatch1.ElapsedMilliseconds} ms");
        }
    }
}
