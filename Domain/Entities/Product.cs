using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string SKU { get; private set; }
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }


        public static Product Create(string name, string sku, decimal price, string? description = null)
        {
            return new Product
            {
                Id = Guid.NewGuid(),
                Name = name,
                SKU = sku,
                Price = price,
                Description = description,
                Quantity = 0,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void AddStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive");
            Quantity += quantity;
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be positive");
            if (quantity > Quantity)
                throw new InvalidOperationException("Insufficient stock");
            Quantity -= quantity;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
