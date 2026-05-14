using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Queries.Login
{
    public class Response
    {
        public string Token { get; set; }
        public string FullName { get; set; }
        public Guid UserId { get; set; }
    }
}
