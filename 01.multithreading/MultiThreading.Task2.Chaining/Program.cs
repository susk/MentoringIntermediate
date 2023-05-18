/*
 * 2. Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    public class Program
    {
        private const int Count = 10;
        private const int Range = 100;
        private static readonly Random Rand = new Random(Range);

        public static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2. Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            var crtArrTask = Task.Factory.StartNew(CreateArray);
            var taskMult = crtArrTask.ContinueWith(
                arrTask =>
            {
                    var multArray = MultiplyArrayByScalar(arrTask.Result, Rand.Next(Range));
                    return multArray;
            }, TaskContinuationOptions.OnlyOnRanToCompletion).ContinueWith(
                multArrTask =>
            {
                    var sortArray = SortArray(multArrTask.Result);
                    return sortArray;
            }, TaskContinuationOptions.OnlyOnRanToCompletion).ContinueWith(
                sortArrTask =>
            {
                    AvgNumInArray(sortArrTask.Result);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            Console.ReadLine();
        }

        private static int[] CreateArray()
        {
            int[] arr = new int[Count];

            for (var i = 0; i < Count; i++)
            {
                arr[i] = Rand.Next(Range);
            }

            Console.WriteLine("The array is created:");
            WriteArray(arr);
            return arr;
        }

        private static long[] MultiplyArrayByScalar(int[] arr, int scalar)
        {
            long[] result = new long[Count];
            for (var i = 0; i < Count; i++)
            {
                result[i] = (long)arr[i] * scalar;
            }

            Console.WriteLine($"The array is multiplied by {scalar}:");
            WriteArray(result);
            return result;
        }

        private static long[] SortArray(long[] arr)
        {
            Array.Sort(arr);

            Console.WriteLine("The array is sorted: ");
            WriteArray(arr);
            return arr;
        }

        private static long AvgNumInArray(long[] arr)
        {
            long sum = 0;
            long average;
            foreach (var num in arr)
            {
                sum += num;
            }

            try
            {
                average = sum / arr.Length;
                Console.WriteLine($"The average of the array is:\n{average}");
            }
            catch (DivideByZeroException e)
            {
                Console.Write("Error: " + e.Message);
                throw e;
            }

            return average;
        }

        private static void WriteArray(int[] arr)
        {
            foreach (var num in arr)
            {
                Console.Write(num + " ");
            }

            Console.WriteLine("\n");
        }

        private static void WriteArray(long[] arr)
        {
            foreach (var num in arr)
            {
                Console.Write(num + " ");
            }

            Console.WriteLine("\n");
        }
    }
}
