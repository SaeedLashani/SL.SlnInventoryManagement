using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using MediatR;

namespace Application.Products.Queries.GetProducts
{
    public class Query:IRequest<List<Response>>
    {
    }
}
