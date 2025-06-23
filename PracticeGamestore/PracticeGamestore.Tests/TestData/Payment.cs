using PracticeGamestore.Business.DataTransferObjects.Payment;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.Business.Options;

namespace PracticeGamestore.Tests.TestData;

public class Payment
{
    public static readonly PaymentOptions PaymentOptions = new ()
    {
        Urls = new ()
        {
            { PaymentMethod.Iban, "https://localhost:5001/externalPayment/iban" },
            { PaymentMethod.CreditCard, "https://localhost:5001/externalPayment/card" },
            { PaymentMethod.Ibox, "https://localhost:5001/externalPayment/ibox" }
        }
    };
    
    public static readonly object[] PaymentServiceTestData =
    [
        new object[] { PaymentMethod.Iban, new IbanDto { Iban = "some iban" } },
        new object[] { PaymentMethod.CreditCard, new CreditCardDto { Number = "1234567890123456", Cvc = "233", ExpirationDate = "09/28" } },
        new object[] { PaymentMethod.Ibox, new IboxDto { TransactionId = "some account" } }
    ];
}