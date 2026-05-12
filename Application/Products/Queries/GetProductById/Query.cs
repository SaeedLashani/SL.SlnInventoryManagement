using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Products.Queries.GetProductById
{
    public class Query:IRequest<Response>
    {
        public Guid Id { get; set; }
    }
}
