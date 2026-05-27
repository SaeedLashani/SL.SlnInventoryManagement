using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.DTOs;
using MediatR;

namespace Application.Products.Queries.SearchInElastic
{
    public class Query:IRequest<IEnumerable<ProductSearchResult>>
    {
        public string query { get; set; }
    }
}
