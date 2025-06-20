using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.Models.Payment;

namespace PracticeGamestore.Tests.TestData;

public class Payment
{
    public static PaymentRequestModel GenerateIbanPaymentRequestModel()
    {
        return new()
        {
            Type = PaymentMethod.Iban,
            Iban = "UA903052992990004149123456789"
        };
    }
    
    public static PaymentRequestModel GenerateCardPaymentRequestModel()
    {
        return new()
        {
            Type = PaymentMethod.Card,
            Card = new CardInfoDto
            {
                Number = "4111111111111111",
                ExpirationDate = "12/30",
                Cvc = "123"
            }
        };
    }
    
    public static PaymentRequestModel GenerateIboxPaymentRequestModel()
    {
        return new()
        {
            Type = PaymentMethod.Ibox,
            Ibox = Guid.NewGuid()
        };
    }
}