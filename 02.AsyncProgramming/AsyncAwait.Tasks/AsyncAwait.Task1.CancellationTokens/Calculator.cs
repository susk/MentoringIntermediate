﻿using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens;

internal static class Calculator
{
    // todo: change this method to support cancellation token
    public static async Task<long> CalculateAsync(int n, CancellationToken token)
    {
        long sum = 0;

        for (var i = 0; i < n; i++)
        {
            if (token.IsCancellationRequested)
            {
                sum = 0;
                return await Task.FromCanceled<long>(token);
            }

            sum = sum + (i + 1);
            await Task.Delay(100, token);
        }

        return sum;
    }
}
