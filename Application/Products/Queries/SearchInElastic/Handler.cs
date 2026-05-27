using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Products.Queries.SearchInElastic
{
    public class Handler : IRequestHandler<Query, IEnumerable<ProductSearchResult>>
    {
        private readonly ISearchService _searchService;

        public Handler(ISearchService searchService)
        {
            _searchService = searchService;
        }
        public async Task<IEnumerable<ProductSearchResult>> Handle(Query request,CancellationToken cancellationToken)
        {
            return await _searchService.SearchAsync(request.query, cancellationToken);
        }
    }
}
