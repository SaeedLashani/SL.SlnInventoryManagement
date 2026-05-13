using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Products.Commands.StackOut
{
    public class Command:IRequest<Unit>
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
