using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Products.Queries.GetProductById
{
    public class Handler:IRequestHandler<Query, Response>
    {
        private readonly IProductRepository _repository;

        public Handler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response> Handle(Query request,CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(
            request.Id, cancellationToken);

            if (product is null)
                throw new KeyNotFoundException($"Product {request.Id} not found");
            return new Response
            {
                Id = product.Id,
                Name = product.Name,
                SKU = product.SKU,
                Price = product.Price,
                Quantity = product.Quantity,
                Description = product.Description,
                CreatedAt = product.CreatedAt
            };
        }
    }
}
