using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Moq;
using FluentAssertions;

namespace Test.Product.Queries
{
    public class GetProductsQueryTests
    {
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly Mock<ICacheService> _cacheMock;
        private readonly Application.Products.Queries.GetProducts.Handler _handler;

        public GetProductsQueryTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _cacheMock = new Mock<ICacheService>();
            _handler = new Application.Products.Queries.GetProducts.Handler(
                _repositoryMock.Object,
                _cacheMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllProducts()
        {
            // Arrange
            var products = new List<Domain.Entities.Product>
        {
            Domain.Entities.Product.Create("Product 1", "SKU-001", 10m),
            Domain.Entities.Product.Create("Product 2", "SKU-002", 20m),
        };

            _repositoryMock
                .Setup(r => r.GetAllAsync(default))
                .ReturnsAsync(products);

            Domain.Entities.Product? nullCache = null;
            _cacheMock
                .Setup(c => c.GetAsync<List<Application.Products.Queries.GetProducts.Response>>(
                    It.IsAny<string>(), default))
                .ReturnsAsync((List<Application.Products.Queries.GetProducts.Response>?)null);

            // Act
            var result = await _handler.Handle(new Application.Products.Queries.GetProducts.Query(), default);

            // Assert
            result.Should().HaveCount(2);
        }
    }
}
