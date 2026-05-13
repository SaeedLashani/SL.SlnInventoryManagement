using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using MediatR;

namespace Application.Products.Commands.StackOut
{
    public class Handler:IRequestHandler<Command,Unit>
    {
        private readonly IProductRepository _repository;
        public Handler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var product=await _repository.GetByIdAsync(request.ProductId, cancellationToken);

            if (product is null)
                throw new KeyNotFoundException($"Product {request.ProductId} not found");

            product.RemoveStock(request.Quantity);

            await _repository.UpdateAsync(product, cancellationToken);

            return Unit.Value;
        }
    }
}
