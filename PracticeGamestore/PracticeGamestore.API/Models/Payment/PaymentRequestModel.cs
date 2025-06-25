namespace PracticeGamestore.Models.Payment;

public class PaymentRequestModel
{
    public PaymentIbanModel? Iban { get; set; }
    public PaymentCreditCardModel? CreditCard { get; set; }
    public PaymentIboxModel? Ibox { get; set; }
}