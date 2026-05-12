using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contaxt;

namespace Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetByIdAsync(
            Guid id, CancellationToken cancellationToken = default)
            => await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        public async Task<IEnumerable<Product>> GetAllAsync(
            CancellationToken cancellationToken = default)
            => await _context.Products.ToListAsync(cancellationToken);

        public async Task<bool> ExistsBySkuAsync(
            string sku, CancellationToken cancellationToken = default)
            => await _context.Products
                .AnyAsync(p => p.SKU == sku, cancellationToken);

        public async Task AddAsync(
            Product product, CancellationToken cancellationToken = default)
        {
            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(
            Product product, CancellationToken cancellationToken = default)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
