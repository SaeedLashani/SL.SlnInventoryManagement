using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Users.Commands.Register
{
    public class Handler : IRequestHandler<Command, Guid>
    {
        private readonly IUserRepository _repository;

        public Handler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            if (await _repository.ExistsByEmailAsync(request.Email, cancellationToken))
                throw new InvalidOperationException("Email already exists");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = User.Create(request.Email, passwordHash, request.FullName);

            await _repository.AddAsync(user, cancellationToken);

            return user.Id;
        }
    }
}
