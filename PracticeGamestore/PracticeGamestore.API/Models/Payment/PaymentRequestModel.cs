namespace PracticeGamestore.Models.Payment;

public class PaymentRequestModel
{
    public IbanModel? Iban { get; set; }
    public CreditCardModel? CreditCard { get; set; }
    public IboxModel? Ibox { get; set; }
}