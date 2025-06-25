namespace PracticeGamestore.Models.Payment;

public class CreditCardModel
{
    public required string Number { get; set;}
    public required string ExpirationDate { get; set; }
    public required string Cvc { get; set; }
}