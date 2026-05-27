using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Products.Commands.Create
{
    public class Handler : IRequestHandler<Command, Response>
    {
        private readonly IProductRepository _repository;
        private readonly ISearchService _searchService;

        public Handler(IProductRepository repository, ISearchService searchService)
        {
            _repository = repository;
            _searchService = searchService;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            if (await _repository.ExistsBySkuAsync(request.SKU, cancellationToken))
                throw new InvalidOperationException("A product with the same SKU already exists.");

            var product = Product.Create(request.Name, request.SKU, request.Price, request.Description);
            await _repository.AddAsync(product, cancellationToken);
            await _searchService.IndexProductAsync(product, cancellationToken);
            return new Response { Id = product.Id };
        }
    }
}
