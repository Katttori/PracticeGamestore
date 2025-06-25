namespace PracticeGamestore.Business.DataTransferObjects.Payment;

public class PaymentDto
{
    public IbanDto? Iban { get; set; }
    public CreditCardDto? CreditCard { get; set; }
    public IboxDto? Ibox { get; set; }
}