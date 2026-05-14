using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly ISender _sender;

        public ProductsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Application.Products.Commands.Create.Command command, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { response.Id }, response.Id);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var products = await _sender.Send(
                new Application.Products.Queries.GetProducts.Query(), cancellationToken);
            return Ok(products);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var product = await _sender.Send(
                new Application.Products.Queries.GetProductById.Query() { Id = id }, cancellationToken);
            return Ok(product);
        }

        [HttpPatch("{id:guid}/stock-in")]
        public async Task<IActionResult> StockIn(Guid id,Application.Products.Commands.StockIn.Command command,
            CancellationToken cancelletionToken)
        {
            command.ProductId = id;
            await _sender.Send(command, cancelletionToken);

            return NoContent();
        }

        [HttpPatch("{id:guid}/stock-out")]
        public async Task<IActionResult> StockOut(Guid id,
            Application.Products.Commands.StackOut.Command command, CancellationToken cancellationToken)
        {
            command.ProductId = id;
            await _sender.Send(command, cancellationToken);

            return NoContent();
        }
    }
}
