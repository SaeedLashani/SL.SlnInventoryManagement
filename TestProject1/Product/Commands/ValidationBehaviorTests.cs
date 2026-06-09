using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Behaviors;
using FluentAssertions;
using FluentValidation;

namespace Test.Product.Commands
{
    public class ValidationBehaviorTests
    {
        [Fact]
        public async Task Handle_InvalidCommand_ShouldThrowValidationException()
        {
            // Arrange
            var validators = new List<IValidator<Application.Products.Commands.Create.Command>>
        {
            new Application.Products.Commands.Create.Validator()
        };

            var behavior = new ValidationBehavior<Application.Products.Commands.Create.Command, Application.Products.Commands.Create.Response>(validators);

            var command = new Application.Products.Commands.Create.Command
            {
                Name = "",        // invalid
                SKU = "",         // invalid
                Price = -1        // invalid 
            };

            // Act
            var act = async () => await behavior.Handle(
                command,
                (CancellationToken ct) => Task.FromResult(new Application.Products.Commands.Create.Response() { Id = Guid.NewGuid() }),
                default);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldCallNext()
        {
            // Arrange
            var validators = new List<IValidator<Application.Products.Commands.Create.Command>>
        {
            new Application.Products.Commands.Create.Validator()
        };

            var behavior = new ValidationBehavior<Application.Products.Commands.Create.Command, Application.Products.Commands.Create.Response>(validators);

            var command = new Application.Products.Commands.Create.Command
            {
                Name = "Test Product",
                SKU = "SKU-001",
                Price = 10m
            };

            var nextCalled = false;

            // Act
            await behavior.Handle(
                command,
                (CancellationToken ct) => { nextCalled = true; return Task.FromResult(new Application.Products.Commands.Create.Response() { Id = Guid.NewGuid() }); },
                default);

            // Assert
            nextCalled.Should().BeTrue();
        }
    }
}
