using FluentValidation;
using PracticeGamestore.Models.Order;

namespace PracticeGamestore.Validators;

public class OrderValidator : AbstractValidator<OrderRequestModel>
{
    public OrderValidator()
    {
        RuleFor(x => x.Total)
            .HasCorrectPrice();
        
        RuleFor(x => x.UserEmail)
            .HasCorrectEmail();

        RuleFor(x => x.GameIds)
            .HasCorrectIds();
    }
}