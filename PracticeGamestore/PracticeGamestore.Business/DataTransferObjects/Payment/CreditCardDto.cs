namespace PracticeGamestore.Business.DataTransferObjects.Payment;

public class CreditCardDto
{
    public required string Number { get; set;}
    public required string ExpirationDate { get; set; }
    public required string Cvc { get; set; }
}