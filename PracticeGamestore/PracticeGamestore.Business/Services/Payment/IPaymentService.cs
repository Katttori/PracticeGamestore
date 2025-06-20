using PracticeGamestore.Business.DataTransferObjects.Payment;

namespace PracticeGamestore.Business.Services.Payment;

public interface IPaymentService
{
    Task<bool> PayIbanAsync(IbanDto iban);
    Task<bool> PayCardAsync(CreditCardDto card);
    Task<bool> PayIboxAsync(IboxDto ibox);
}