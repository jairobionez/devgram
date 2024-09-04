using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Devgram.Infra.Context
{
    public class Context : IdentityDbContext<IdentityUser>
    {

        protected Context()
        {

        }

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }
    }
}