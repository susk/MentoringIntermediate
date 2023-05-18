/*
 * 4. Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 *
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    public class Program
    {
        private const int Count = 10;
        private static readonly Semaphore Sem = new Semaphore(initialCount: 0, maximumCount: 1);

        public static void Main(string[] args)
        {
            Console.WriteLine("4. Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            Console.WriteLine("CreateThreadsUsingJoin");
            CreateThreadsUsingJoin(Count);

            Console.WriteLine("CreateThreadsUsingSemaphore");
            CreateThreadsUsingSemaphore(Count);
            Sem.WaitOne(1);

            Console.ReadLine();
        }

        private static void CreateThreadsUsingJoin(object data)
        {
            if (data is not int count)
            {
                throw new ArgumentException("data should be int");
            }

            if (count <= 0)
            {
                return;
            }

            count--;

            Console.WriteLine($"Thread state: {count}");
            Thread newThread = new Thread(CreateThreadsUsingJoin);
            newThread.Start(count);
            newThread.Join();
        }

        private static void CreateThreadsUsingSemaphore(object data)
        {
            if (data is not int count)
            {
                throw new ArgumentException("data should be int");
            }

            if (count <= 0)
            {
                Sem.Release();
                return;
            }

            count--;

            Console.WriteLine($"Thread state: {count}");

            ThreadPool.QueueUserWorkItem(CreateThreadsUsingSemaphore, count);
        }
    }
}
