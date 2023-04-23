/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    public class Program
    {
        private const int size = 10;
        private static List<int> collection = new List<int>();
        private static EventWaitHandle ewhReadyToRead = new EventWaitHandle(false, EventResetMode.AutoReset);
        private static EventWaitHandle ewhReadyToWrite = new EventWaitHandle(true, EventResetMode.AutoReset);

        public static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();
            
            var readThread = new Thread(ReadArr);
            var writeThread = new Thread(WriteArr);

                      
            writeThread.Start();
            readThread.Start();

            writeThread.Join();
            readThread.Join();

            Console.ReadLine();
        }

        private static void WriteArr()
        {
            int i = 0;
            while (i < size)
            {                
                ewhReadyToWrite.WaitOne();                                
                collection.Add(i++);                
                ewhReadyToRead.Set();
            }
        }

        private static void ReadArr()
        {            
            for (var i = 0; i < size; i++)
            {
                ewhReadyToRead.WaitOne();                
                foreach (var el in collection)
                {
                    Console.Write(el + " ");
                }

                Console.WriteLine();
                ewhReadyToWrite.Set();
            }
            
        }
    }
}
