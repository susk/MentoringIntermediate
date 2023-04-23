using System;
using System.Threading;
using System.Threading.Tasks;
using AsyncAwait.Task2.CodeReviewChallenge.Headers;
using CloudServices.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AsyncAwait.Task2.CodeReviewChallenge.Middleware;

public class StatisticMiddleware
{
    private readonly RequestDelegate next;

    private readonly IStatisticService statisticService;

    public StatisticMiddleware(RequestDelegate next, IStatisticService statisticService)
    {
        this.next = next;
        this.statisticService = statisticService ?? throw new ArgumentNullException(nameof(statisticService));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string path = context.Request.Path;

        await this.statisticService.RegisterVisitAsync(path).ConfigureAwait(false);

        long visitCount = await this.statisticService.GetVisitsCountAsync(path);

        context.Response.Headers.Add(
               CustomHttpHeaders.TotalPageVisits,
               visitCount.ToString());

        await this.next(context);
    }
}
