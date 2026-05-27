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
    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IProductRepository _repository;
        private readonly ICacheService _cache;

        public Handler(IProductRepository repository, ICacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            string CacheKey = $"products:{request.Id}";
            var cached = await _cache.GetAsync<Response>(CacheKey, cancellationToken);

            if (cached is not null)
                return cached;

            var product = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (product is null)
                throw new KeyNotFoundException($"Product {request.Id} not found");

            var response = new Response
            {
                Id = product.Id,
                Name = product.Name,
                SKU = product.SKU,
                Price = product.Price,
                Quantity = product.Quantity,
                Description = product.Description,
                CreatedAt = product.CreatedAt
            };

            await _cache.SetAsync(CacheKey, response,TimeSpan.FromMinutes(5), cancellationToken);

            return response;
        }
    }
}
