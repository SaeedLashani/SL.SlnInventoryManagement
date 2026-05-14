using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using MediatR;

namespace Application.Users.Queries.Login
{
    public class Handler:IRequestHandler<Query, Response>
    {
        private readonly IUserRepository _repository;
        private readonly IJwtService _jwtService;
        public Handler(IUserRepository repository, IJwtService jwtService)
        {
            _repository = repository;
            _jwtService = jwtService;
        }

        public async Task<Response> Handle (Query request,CancellationToken cancellationToken)
        {
            var user = await _repository.GetByEmailAsync(request.Email, cancellationToken);

            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new InvalidOperationException("Invalid email or password");

            var token = _jwtService.GenerateToken(user);

            return new Response {
                Token = token,
                FullName = user.FullName,
                UserId = user.Id
            };
        }
    }
}
