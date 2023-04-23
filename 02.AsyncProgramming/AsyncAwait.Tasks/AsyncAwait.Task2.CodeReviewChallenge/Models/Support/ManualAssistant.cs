using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CloudServices.Interfaces;

namespace AsyncAwait.Task2.CodeReviewChallenge.Models.Support;

public class ManualAssistant : IAssistant
{
    private readonly ISupportService supportService;

    public ManualAssistant(ISupportService supportService)
    {
        this.supportService = supportService ?? throw new ArgumentNullException(nameof(supportService));
    }

    public async Task<string> RequestAssistanceAsync(string requestInfo)
    {
        try
        {
            await this.supportService.RegisterSupportRequestAsync(requestInfo).ConfigureAwait(false); //todo
            return await this.supportService.GetSupportInfoAsync(requestInfo)
                .ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            return $"Failed to register assistance request. Please try later. {ex.Message}";
        }
    }
}
