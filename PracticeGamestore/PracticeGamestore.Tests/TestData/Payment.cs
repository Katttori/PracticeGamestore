using PracticeGamestore.Business.DataTransferObjects.Payment;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.Business.Options;
using PracticeGamestore.Models.Payment;

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
  
    public static PaymentRequestModel GeneratePaymentRequestModel(bool? iban = null, bool? creditCard = null, bool? ibox = null)
      {
          return new()
          {
              Iban = iban is not null ? new IbanModel { Iban = "UA903052992990004149123456789" } : null,
              CreditCard = creditCard is not null
                  ? new CreditCardModel
                  {
                      Number = "4111111111111111",
                      ExpirationDate = "12/30",
                      Cvc = "123"
                  }
                  : null,
              Ibox = ibox is not null ? new IboxModel { TransactionId = Guid.NewGuid() } : null
          };
      }

    public static PaymentDto GeneratePaymentDto()
    {
        return new()
        {
            Iban = new IbanDto { Iban = "UA903052992990004149123456789" },
            CreditCard = new CreditCardDto
            {
                Number = "4111111111111111",
                ExpirationDate = "12/30",
                Cvc = "123"
            },
            Ibox = new IboxDto { TransactionId = Guid.NewGuid() }
        };
    }
}