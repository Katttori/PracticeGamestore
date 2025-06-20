using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PracticeGamestore.Business.DataTransferObjects.Payment;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.Business.Options;

namespace PracticeGamestore.Business.Services.Payment;

public class PaymentService(HttpClient httpClient,
                            IOptions<PaymentOptions> paymentOptions,
                            ILogger<PaymentService> logger) : IPaymentService
{
    private readonly PaymentOptions _paymentOptions = paymentOptions.Value;
    
    public async Task<bool> PayIbanAsync(IbanDto iban)
    {
        return await ProcessPaymentAsync(iban, PaymentMethod.Iban);
    }

    public async Task<bool> PayCardAsync(CreditCardDto card)
    {
        return await ProcessPaymentAsync(card, PaymentMethod.CreditCard);
    }

    public async Task<bool> PayIboxAsync(IboxDto ibox)
    {
        return await ProcessPaymentAsync(ibox, PaymentMethod.Ibox);
    }

    private async Task<bool> ProcessPaymentAsync<T>(T paymentModel, PaymentMethod paymentMethod)
    {
        var endpoint = _paymentOptions.Options.GetValueOrDefault(paymentMethod);
        try
        {
            var response = await httpClient.PostAsJsonAsync(endpoint, paymentModel);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Successfully processed {PaymentMethod} payment. Response: {Response}",
                    paymentMethod.ToString(), response); 
                return true;
            }    
            logger.LogWarning("{PaymentMethod} payment failed with status {StatusCode}. Response: {Response}", 
                paymentMethod.ToString(), response.StatusCode, responseContent);
            return false;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "{PaymentMethod} payment failed after all retries", paymentMethod.ToString());
            return false;
        }
        catch (TaskCanceledException ex)
        {
            logger.LogError(ex, "{PaymentMethod} payment timed out", paymentMethod.ToString());
            return false;
        }
    }
}