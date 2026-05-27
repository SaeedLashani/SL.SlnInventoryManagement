using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.Products.Queries.SearchInElastic
{
    public class Validation : AbstractValidator<Query>
    {
        public Validation()
        {
            RuleFor(x => x.query).NotEmpty().MinimumLength(2);
        }
    }
}
