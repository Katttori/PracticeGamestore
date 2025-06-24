using PracticeGamestore.Business.DataTransferObjects;
using PracticeGamestore.Business.DataTransferObjects.Payment;
using PracticeGamestore.Models.Payment;

namespace PracticeGamestore.Mappers;

public static class PaymentMappingExtensions
{
    public static PaymentDto MapToPaymentDto(this PaymentRequestModel model)
    {
        return new()
        {
            Iban = model.Iban != null 
                ? new IbanDto { Iban = model.Iban.Iban } 
                : null,

            CreditCard = model.CreditCard != null
                ? new CreditCardDto
                {
                    Number = model.CreditCard.Number,
                    ExpirationDate = model.CreditCard.ExpirationDate,
                    Cvc = model.CreditCard.Cvc
                }
                : null,

            Ibox = model.Ibox != null 
                ? new IboxDto { TransactionId = model.Ibox.TransactionId } 
                : null
        };
    }
}