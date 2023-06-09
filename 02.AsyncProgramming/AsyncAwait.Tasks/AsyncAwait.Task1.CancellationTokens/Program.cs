﻿/*
* Study the code of this application to calculate the sum of integers from 0 to N, and then
* change the application code so that the following requirements are met:
* 1. The calculation must be performed asynchronously.
* 2. N is set by the user from the console. The user has the right to make a new boundary in the calculation process,
* which should lead to the restart of the calculation.
* 3. When restarting the calculation, the application should continue working without any failures.
*/

using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens;

internal class Program
{
    private static CancellationTokenSource cts = new CancellationTokenSource();
    private static Task calcTask;

    /// <summary>
    /// The Main method should not be changed at all.
    /// </summary>
    /// <param name="args">args.</param>
    private static void Main(string[] args)
    {
        Console.WriteLine("Mentoring program L2. Async/await.V1. Task 1");
        Console.WriteLine("Calculating the sum of integers from 0 to N.");
        Console.WriteLine("Use 'q' key to exit...");
        Console.WriteLine();

        Console.WriteLine("Enter N: ");

        var input = Console.ReadLine();
        Task calcTask = null;

        while (input.Trim().ToUpper() != "Q")
        {
            if (int.TryParse(input, out var n))
            {
                CalculateSum(n);
            }
            else
            {
                Console.WriteLine($"Invalid integer: '{input}'. Please try again.");
                Console.WriteLine("Enter N: ");
            }

            input = Console.ReadLine();
        }

        Console.WriteLine("Press any key to continue");
        Console.ReadLine();
    }

    private static async Task CalculateSum(int n)
    {
        Console.WriteLine($"The task for {n} started... Enter N to cancel the request:");

        cts.Cancel();
        if (calcTask != null)
        {
            await calcTask;
        }

        cts.Dispose();
        cts = new CancellationTokenSource();

        calcTask = Calculator.CalculateAsync(n, cts.Token)
            .ContinueWith(parentTask =>
            {
                if (parentTask.IsCanceled)
                {
                    Console.WriteLine($"The task is cancelled...");
                }
                else
                {
                    Console.WriteLine($"Sum for {n} = {parentTask.Result}.");
                }
            });
    }
}
