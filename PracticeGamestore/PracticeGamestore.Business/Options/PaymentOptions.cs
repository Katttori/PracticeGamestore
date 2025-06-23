using PracticeGamestore.Business.Enums;

namespace PracticeGamestore.Business.Options;

public class PaymentOptions
{
    public const string SectionName = "Payment";

    public required Dictionary<PaymentMethod, string> Urls { get; set; } = new();
}