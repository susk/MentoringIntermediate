using System;
using System.Diagnostics;
using System.Threading.Tasks;
using AsyncAwait.Task2.CodeReviewChallenge.Models;
using AsyncAwait.Task2.CodeReviewChallenge.Models.Support;
using AsyncAwait.Task2.CodeReviewChallenge.Services;
using Microsoft.AspNetCore.Mvc;

namespace AsyncAwait.Task2.CodeReviewChallenge.Controllers;

public class HomeController : Controller
{
    private readonly IAssistant assistant;

    private readonly IPrivacyDataService privacyDataService;

    public HomeController(IAssistant assistant, IPrivacyDataService privacyDataService)
    {
        this.assistant = assistant ?? throw new ArgumentNullException(nameof(assistant));
        this.privacyDataService = privacyDataService ?? throw new ArgumentNullException(nameof(privacyDataService));
    }

    public ActionResult Index()
    {
        return this.View();
    }

    public async Task<IActionResult> Privacy()
    {
        this.ViewBag.Message = await this.privacyDataService.GetPrivacyDataAsync().ConfigureAwait(false);
        return this.View();
    }

    public async Task<IActionResult> Help()
    {
        this.ViewBag.RequestInfo = await this.assistant.RequestAssistanceAsync("guest").ConfigureAwait(false);
        return this.View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
    }
}
