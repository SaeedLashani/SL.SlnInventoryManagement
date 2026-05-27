using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Nest;

namespace Infrastructure.Services
{
    public class SearchService : ISearchService
    {
        private readonly IElasticClient _client;
        private const string IndexName = "products";

        public SearchService(IElasticClient client)
        {
            _client = client;
        }

        public async Task IndexProductAsync(Product product, CancellationToken cancellationToken = default)
        {
            await _client.IndexDocumentAsync(new ProductSearchResult
            {
                Id = product.Id,
                Name = product.Name,
                SKU = product.SKU,
                Price = product.Price,
                Quantity = product.Quantity,
                Description = product.Description
            });
        }

        public async Task<IEnumerable<ProductSearchResult>> SearchAsync(string query, CancellationToken cancellationToken = default)
        {
            var response = await _client.SearchAsync<ProductSearchResult>(s => s
                .Index(IndexName).Query(q => q.MultiMatch(m => m.Fields(f => f.Field(p => p.Name).Field(p => p.SKU).Field(p => p.Description)).Query(query))));

            return response.Documents;
        }
    }
}
