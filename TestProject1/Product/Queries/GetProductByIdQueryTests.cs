using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using FluentAssertions;
using Moq;

namespace Test.Product.Queries
{
    public class GetProductByIdQueryTests
    {
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly Mock<ICacheService> _cacheMock;
        private readonly Application.Products.Queries.GetProductById.Handler _handler;

        public GetProductByIdQueryTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _cacheMock = new Mock<ICacheService>();
            _handler = new Application.Products.Queries.GetProductById.Handler(
                _repositoryMock.Object,
                _cacheMock.Object);
        }

        [Fact]
        public async Task Handle_ValidId_ShouldReturnProduct()
        {
            // Arrange
            var product = Domain.Entities.Product.Create("Test", "SKU-001", 10m);

            _repositoryMock
                .Setup(r => r.GetByIdAsync(product.Id, default))
                .ReturnsAsync(product);

            _cacheMock
                .Setup(c => c.GetAsync<Application.Products.Queries.GetProductById.Response>(
                    It.IsAny<string>(), default))
                .ReturnsAsync((Application.Products.Queries.GetProductById.Response?)null);

            // Act
            var result = await _handler.Handle(
                new Application.Products.Queries.GetProductById.Query { Id = product.Id }, default);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Test");
        }

        [Fact]
        public async Task Handle_InvalidId_ShouldThrowException()
        {
            // Arrange
            Domain.Entities.Product? nullProduct = null;
            _repositoryMock
                .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), default))
                .ReturnsAsync(nullProduct);

            _cacheMock
                .Setup(c => c.GetAsync<Application.Products.Queries.GetProductById.Response>(
                    It.IsAny<string>(), default))
                .ReturnsAsync((Application.Products.Queries.GetProductById.Response?)null);

            // Act
            var act = async () => await _handler.Handle(
                new Application.Products.Queries.GetProductById.Query { Id = Guid.NewGuid() }, default);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }
    }
}
