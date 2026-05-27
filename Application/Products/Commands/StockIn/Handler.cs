using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using MediatR;

namespace Application.Products.Commands.StockIn
{
    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IProductRepository _repository;
        private readonly ICacheService _cache;
        private const string CacheKey = "products:all";

        public Handler(IProductRepository repository, ICacheService cache)
        {
            _repository = repository;
            _cache = cache;
        }
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.ProductId, cancellationToken);

            if (product is null)
                throw new KeyNotFoundException($"Product {request.ProductId} not found");

            product.AddStock(request.Quantity);

            await _repository.UpdateAsync(product, cancellationToken);

            await _cache.RemoveAsync(CacheKey, cancellationToken);
            await _cache.RemoveAsync($"products:{request.ProductId}", cancellationToken);

            return Unit.Value;
        }
    }
}
