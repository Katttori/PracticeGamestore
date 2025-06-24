using System.Globalization;
using FluentValidation;
using PracticeGamestore.Models.Payment;
using PracticeGamestore.Business.Constants;

namespace PracticeGamestore.Validators;

public class PaymentValidator : AbstractValidator<PaymentRequestModel>
{
    public PaymentValidator()
    {
        RuleFor(x => x)
            .Must(HasAtLeastOnePaymentMethod)
            .WithMessage(ErrorMessages.PropertyRequired("At least one payment method (IBAN, Card, or IBox)"));

        When(x => x.Iban is not null, () =>
        {
            RuleFor(x => x.Iban!.Iban)
                .NotEmpty().WithMessage(ErrorMessages.PropertyRequired("IBAN"))
                .Matches(@"^UA\d{27}$")
                .WithMessage(ErrorMessages.InvalidIbanFormat);
        });

        When(x => x.Iban is null && x.CreditCard is not null, () =>
        {
            RuleFor(x => x.CreditCard)
                .NotNull().WithMessage(ErrorMessages.PropertyRequired("Card information"));

            RuleFor(x => x.CreditCard!.Number)
                .NotEmpty().WithMessage(ErrorMessages.PropertyRequired("Card number"))
                .CreditCard().WithMessage(ErrorMessages.InvalidCardNumber);

            RuleFor(x => x.CreditCard!.ExpirationDate)
                .NotEmpty().WithMessage(ErrorMessages.PropertyRequired("Expiration date"))
                .Matches(@"^(0[1-9]|1[0-2])\/\d{2}$")
                .WithMessage(ErrorMessages.InvalidExpirationDate)
                .Must(BeAValidFutureDate)
                .WithMessage(ErrorMessages.InvalidExpirationDate);

            RuleFor(x => x.CreditCard!.Cvc)
                .NotEmpty().WithMessage(ErrorMessages.PropertyRequired("CVC"))
                .Matches(@"^\d{3}$").WithMessage(ErrorMessages.InvalidCvc);
        });

        When(x => x.Iban is null && x.CreditCard is null && x.Ibox is not null, () =>
        {
            RuleFor(x => x.Ibox!.TransactionId)
                .NotEmpty().WithMessage(ErrorMessages.PropertyRequired("IBox ID"))
                .Must(id => id != Guid.Empty)
                .WithMessage(ErrorMessages.InvalidIbox);
        });
    }

    private static bool HasAtLeastOnePaymentMethod(PaymentRequestModel model)
    {
        return model.Iban is not null || model.CreditCard is not null || model.Ibox is not null;
    }

    private static bool BeAValidFutureDate(string expirationDate)
    {
        if (!DateTime.TryParseExact(expirationDate, "MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            return false;

        var lastDayOfMonth = new DateTime(parsedDate.Year, parsedDate.Month, DateTime.DaysInMonth(parsedDate.Year, parsedDate.Month));
        return lastDayOfMonth >= DateTime.Today;
    }
}