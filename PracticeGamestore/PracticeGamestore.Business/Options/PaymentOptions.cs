using PracticeGamestore.Business.Enums;

namespace PracticeGamestore.Business.Options;

public class PaymentOptions
{
    public const string SectionName = "Payment";

    public required string BaseUrl { get; set; }
    public required Dictionary<PaymentMethod, string> Endpoints { get; set; } = new();
}