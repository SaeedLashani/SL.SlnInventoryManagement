using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using MediatR;

namespace Application.Products.Queries.GetProducts
{
    public class Handler:IRequestHandler<Query, List<Response>>
    {
        private readonly IProductRepository _repository;
        private readonly ICacheService _cache;
        private const string CacheKey = "products:all";

        public Handler(IProductRepository repository,ICacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<List<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            var cached = await _cache.GetAsync<List<Response>>(CacheKey, cancellationToken);

            if (cached is not null)
                return cached;

            var products = await _repository.GetAllAsync(cancellationToken);
            var response = products.Select(p => new Response
            {
                Id = p.Id,
                Name = p.Name,
                SKU = p.SKU,
                Price = p.Price,
                Quantity = p.Quantity
            }).ToList();

            await _cache.SetAsync(CacheKey, response, TimeSpan.FromMinutes(5), cancellationToken);
            return response;
        }
    }
}
