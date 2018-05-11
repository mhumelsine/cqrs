using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Isf.Core.Cqrs
{
    public class EventDbContext : DbContext
    {
        DbSet<DomainEvent> DomainEvents { get; set; }

        public EventDbContext(DbContextOptions options) : base(options)
        {

        }
    }


}
