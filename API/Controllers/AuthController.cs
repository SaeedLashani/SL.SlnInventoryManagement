using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Application.Users.Commands.Register.Command command, CancellationToken cancellationToken)
        {
            var id = await _sender.Send(command, cancellationToken);
            return Ok(id);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Application.Users.Queries.Login.Query query, CancellationToken cancellationToken)
        {
            var response = await _sender.Send(query, cancellationToken);
            return Ok(response);
        }
    }
}
