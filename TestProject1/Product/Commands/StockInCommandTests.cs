using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using FluentAssertions;
using Moq;

namespace Test.Product.Commands
{
    public class StockInCommandTests
    {
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly Mock<ICacheService> _cacheMock;
        private readonly Mock<ISearchService> _searchServiceMock;
        private readonly Application.Products.Commands.StockIn.Handler _handler;

        public StockInCommandTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _cacheMock = new Mock<ICacheService>();
            _searchServiceMock = new Mock<ISearchService>();
            _handler = new Application.Products.Commands.StockIn.Handler(
                _repositoryMock.Object,
                _cacheMock.Object,
                _searchServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldAddStock()
        {
            // Arrange
            var product = Domain.Entities.Product.Create("Test", "SKU-001", 10m);
            var command = new Application.Products.Commands.StockIn.Command
            {
                ProductId = product.Id,
                Quantity = 5
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(command.ProductId, default))
                .ReturnsAsync(product);

            // Act
            await _handler.Handle(command, default);

            // Assert
            product.Quantity.Should().Be(5);
            _repositoryMock.Verify(r => r.UpdateAsync(
                It.IsAny<Domain.Entities.Product>(), default), Times.Once);
        }

        [Fact]
        public async Task Handle_ProductNotFound_ShouldThrowException()
        {
            // Arrange
            var command = new Application.Products.Commands.StockIn.Command
            {
                ProductId = Guid.NewGuid(),
                Quantity = 5
            };

             _repositoryMock.Setup(r => r.GetByIdAsync(command.ProductId, default))
                .ReturnsAsync((Domain.Entities.Product?)null);

            // Act
            var act = async () => await _handler.Handle(command, default);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }
    }
}
