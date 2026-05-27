using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.DTOs;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISearchService
    {
        Task IndexProductAsync(Product product, CancellationToken cancellationToken = default);
        Task<IEnumerable<ProductSearchResult>> SearchAsync(string query, CancellationToken cancellationToken = default);
    }
}
