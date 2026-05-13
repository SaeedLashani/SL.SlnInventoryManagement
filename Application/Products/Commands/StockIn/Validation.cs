using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Products.Commands.StockIn
{
    public class Validation:AbstractValidator<Command>
    {
        public Validation()
        {
            RuleFor(x=> x.ProductId).NotEmpty().WithMessage("Product ID is required")
                .Must(id => id != Guid.Empty).WithMessage("Product ID must be a valid GUID");

            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero");
        }
    }
}
