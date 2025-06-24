using System.ComponentModel.DataAnnotations;

namespace PracticeGamestore.Business.Options;

public class RetryPolicyOptions
{
    public const string SectionName = "RetryPolicy";

    [Range(1, 300)]
    public required int TimeoutSeconds { get; set; }
    
    [Range(1, 10)]
    public required int MaxRetryAttempts { get; set; }
    
    [Range(100, 5000)]
    public required int RetryDelayMs { get; set; }
    
    [Range(1, 5)]
    public required double BackoffMultiplier { get; set; }
    
    [Range(1000, 60000)]
    public required int MaxRetryDelayMs { get; set; }
}