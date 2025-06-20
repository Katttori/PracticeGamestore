using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Enums;

namespace PracticeGamestore.Models.Payment;

public class PaymentRequestModel
{
    public PaymentMethod Type { get; set; }
    public string? Iban { get; set; }
    public CardInfoDto? Card { get; set; }
    public Guid? Ibox { get; set; }
}