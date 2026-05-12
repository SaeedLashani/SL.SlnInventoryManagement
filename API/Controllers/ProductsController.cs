using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
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
    }
}
