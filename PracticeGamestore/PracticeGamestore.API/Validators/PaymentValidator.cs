using System.Globalization;
using FluentValidation;
using PracticeGamestore.Business.Constants;
using PracticeGamestore.Business.Enums;
using PracticeGamestore.Models.Payment;

namespace PracticeGamestore.Validators;

public class PaymentValidator : AbstractValidator<PaymentRequestModel>
{
    public PaymentValidator()
    {
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage(ErrorMessages.PropertyRequired("Payment type"))
            .IsInEnum().WithMessage(ErrorMessages.InvalidPaymentType);

        When(x => x.Type == PaymentMethod.Iban, () =>
        {
            RuleFor(x => x.Iban)
                .NotEmpty().WithMessage(ErrorMessages.PropertyRequired("IBAN"))
                .Matches(@"^UA\d{27}$")
                .WithMessage(ErrorMessages.InvalidIbanFormat);
        });

        When(x => x.Type == PaymentMethod.Card, () =>
        {
            RuleFor(x => x.Card)
                .NotNull().WithMessage(ErrorMessages.PropertyRequired("Card information"));

            RuleFor(x => x.Card!.Number)
                .NotEmpty().WithMessage(ErrorMessages.PropertyRequired("Card number"))
                .CreditCard()
                .WithMessage(ErrorMessages.InvalidCardNumber);

            RuleFor(x => x.Card!.ExpirationDate)
                .NotEmpty().WithMessage(ErrorMessages.PropertyRequired("Expiration date"))
                .Matches(@"^(0[1-9]|1[0-2])\/\d{2}$")
                .Must(BeAValidFutureDate)
                .WithMessage(ErrorMessages.InvalidExpirationDate);

            RuleFor(x => x.Card!.Cvc)
                .NotEmpty().WithMessage(ErrorMessages.PropertyRequired("CVC"))
                .Matches(@"^\d{3}$")
                .WithMessage(ErrorMessages.InvalidCvc);
        });

        When(x => x.Type == PaymentMethod.Ibox, () =>
        {
            RuleFor(x => x.Ibox)
                .NotNull().WithMessage(ErrorMessages.PropertyRequired("IBox ID"))
                .Must(v => v != Guid.Empty)
                .WithMessage(ErrorMessages.InvalidIbox);
        });
    }
    
    private bool BeAValidFutureDate(string expirationDate)
    {
        if (!DateTime.TryParseExact(expirationDate, "MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            return false;
        }

        var lastDayOfMonth = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        return lastDayOfMonth >= DateTime.Today;
    }
}