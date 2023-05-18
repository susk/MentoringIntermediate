namespace AsyncAwait.Task2.CodeReviewChallenge.Models;

public class ErrorViewModel
{
    public string RequestId { get; set; }

    public bool HasRequestId => !string.IsNullOrEmpty(this.RequestId);
}