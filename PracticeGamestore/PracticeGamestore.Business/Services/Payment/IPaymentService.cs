using PracticeGamestore.Business.DataTransferObjects;

namespace PracticeGamestore.Business.Services.Payment;

public interface IPaymentService
{
    Task<bool> PayIbanAsync(string iban);
    Task<bool> PayCardAsync(CardInfoDto card);
    Task<bool> PayIboxAsync(Guid ibox);
}