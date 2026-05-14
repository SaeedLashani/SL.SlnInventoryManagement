using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string FullName { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public static User Create(string email,string passwordHash,string fullName)
        {  
            return new User {Id=Guid.NewGuid(), CreatedAt = DateTime.UtcNow, Email = email, PasswordHash = passwordHash, FullName = fullName };
        }
    }
}
