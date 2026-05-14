using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Application.Users.Queries.Login
{
    public class Query:IRequest<Response>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
