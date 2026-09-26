using FluentValidation;
using Ordering.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Validators
{
    public class CheckOutOrderCommandValidatorV2 : AbstractValidator<CheckOutOrderCommandV2>
    {
        public CheckOutOrderCommandValidatorV2()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("{UserName} is required.")
                .NotNull()
                .MaximumLength(70).WithMessage("{UserName} must not exceed 70 characters.");

            RuleFor(x => x.TotaPrice)
                .GreaterThan(0).WithMessage("{TotalPrice} must be greater than zero.");
        }
        }
}
