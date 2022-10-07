using System;
using System.Diagnostics;
using System.Collections;

namespace ConsoleApp1
{
    internal class Timing
    {
        TimeSpan duration;
        TimeSpan[] threads;
        public Timing()
        {
            duration = new TimeSpan(0);
            threads = new TimeSpan[Process.GetCurrentProcess().Threads.Count];
        }

        public void StartTime()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            for (int i = 0; i < threads.Length; i++)
                threads[i] = Process.GetCurrentProcess().Threads[i].UserProcessorTime;
        }

        public void StopTime()
        {
            TimeSpan tmp;
            for (int i = 0; i < threads.Length; i++)
            {
                tmp = Process.GetCurrentProcess().Threads[i].UserProcessorTime.Subtract(threads[i]);
                if (tmp > TimeSpan.Zero)
                    duration = tmp;
            }
        }

        public TimeSpan Result()
        {
            return duration;
        }
    }

    class Program
    {
        public static void BubleSort(int[] a)
        {
            int N = a.Length;
            for (int i = 1; i < N; i++)
                for (int j = N - 1; j >= N; j--)
                {
                    if (a[j - 1] > a[j])
                    {
                        int t = a[j - 1];
                        a[j - 1] = a[j];
                        a[j] = t;
                    }
                }
        }

        static int SearchBarrier(int[] a, int x)
        {
            int L = a.Length;
            Array.Resize<int>(ref a, ++L);
            a[L - 1] = x;
            int i = 0;
            while (a[i] != x)
                i++;
            if (i < L - 1)
                return i;
            else
                return -1;
        }

        static int SearchBinary(int[] a, int x)
        {
            int middle, left = 0, right = a.Length - 1;
            do
            {
                middle = (left + right) / 2;
                if (x > a[middle])
                    left = middle + 1;
                else
                    right = middle - 1;
            }
            while ((a[middle] != x) && (left <= right));
            if (a[middle] == x)
                return middle;
            else
                return -1;
        }

        static int SearchBarrierHash(Hashtable hash, int x)
        {
            foreach (int key in hash.Keys)
                if (hash[key].GetHashCode() == x)
                    return key;
            return -1;
        }

        static void Main(string[] args)
        {
            const int n = 910000;             //пришлось сильно увеличить количество чисел в массиве,                                            
            int[] a = new int[n];              //так как программа выполнялась быстрее 1/100000 мс и timespan не показывал результат
            Random rnd = new Random();
            Hashtable hash = new Hashtable(n);

            for (int i = 0; i < n; i++)
            {
                a[i] = rnd.Next() % 500;
                hash.Add(i, a[i]);
            }

            Timing objT = new Timing();
            Stopwatch stpWatch = new Stopwatch();

            objT.StartTime();
            stpWatch.Start();

            SearchBarrier(a, 600);

            stpWatch.Stop();
            objT.StopTime();

            Console.WriteLine("StopWatch: " + stpWatch.Elapsed.ToString());
            Console.WriteLine("Timing: " + objT.Result().ToString());


            Timing objT1 = new Timing();
            Stopwatch stpWatch1 = new Stopwatch();

            objT1.StartTime();
            stpWatch1.Start();

            BubleSort(a);

            stpWatch1.Stop();
            objT1.StopTime();

            Console.WriteLine("StopWatch: " + stpWatch1.Elapsed.ToString());
            Console.WriteLine("Timing: " + objT1.Result().ToString());


            Timing objT2 = new Timing();
            Stopwatch stpWatch2 = new Stopwatch();

            objT2.StartTime();
            stpWatch2.Start();

            SearchBinary(a, 600);

            stpWatch2.Stop();
            objT2.StopTime();

            Console.WriteLine("StopWatch: " + stpWatch2.Elapsed.ToString());
            Console.WriteLine("Timing: " + objT2.Result().ToString());

            Timing objT3 = new Timing();
            Stopwatch stpWatch3 = new Stopwatch();

            objT3.StartTime();
            stpWatch3.Start();

            Console.WriteLine(SearchBarrierHash(hash, 100));

            stpWatch3.Stop();
            objT3.StopTime();

            Console.WriteLine("StopWatch: " + stpWatch3.Elapsed.ToString());
            Console.WriteLine("Timing: " + objT3.Result().ToString());

            //не успел реализовать метод цепочки для избежания коллизии при поиске элемента
        }

    }
}