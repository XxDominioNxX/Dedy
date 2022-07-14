using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

namespace Dedy.Models
{
    public class AuthorizationContext : IdentityDbContext<User>
    {
        public AuthorizationContext(DbContextOptions<AuthorizationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
