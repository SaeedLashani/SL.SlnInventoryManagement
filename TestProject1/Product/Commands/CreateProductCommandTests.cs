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
    public class CreateProductCommandTests
    {
        private readonly Mock<IProductRepository> _repositoryMock;
        private readonly Mock<ISearchService> _searchServiceMock;
        private readonly Application.Products.Commands.Create.Handler _handler;

        public CreateProductCommandTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
            _searchServiceMock = new Mock<ISearchService>();
            _handler = new Application.Products.Commands.Create.Handler(
                _repositoryMock.Object,
                _searchServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldCreateProduct()
        {
            // Arrange
            var command = new Application.Products.Commands.Create.Command
            {
                Name = "Test Product",
                SKU = "TEST-001",
                Price = 99.99m,
                Description = "Test Description"
            };

            _repositoryMock
                .Setup(r => r.ExistsBySkuAsync(command.SKU, default))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            result.Should().NotBe(new Application.Products.Commands.Create.Response() { Id = Guid.Empty });
            _repositoryMock.Verify(r => r.AddAsync(
                It.IsAny<Domain.Entities.Product>(), default), Times.Once);
        }

        [Fact]
        public async Task Handle_DuplicateSKU_ShouldThrowException()
        {
            // Arrange
            var command = new Application.Products.Commands.Create.Command
            {
                Name = "Test Product",
                SKU = "DUPLICATE-001",
                Price = 99.99m
            };

            _repositoryMock
                .Setup(r => r.ExistsBySkuAsync(command.SKU, default))
                .ReturnsAsync(true);

            // Act
            var act = async () => await _handler.Handle(command, default);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
