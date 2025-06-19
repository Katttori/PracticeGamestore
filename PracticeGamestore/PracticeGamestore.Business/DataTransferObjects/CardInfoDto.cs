namespace PracticeGamestore.Business.DataTransferObjects;

public class CardInfoDto
{
    public required string Number { get; set; }
    public required string ExpirationDate { get; set; }
    public required string Cvc { get; set; }
}