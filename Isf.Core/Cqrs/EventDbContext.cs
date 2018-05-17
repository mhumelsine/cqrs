using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Isf.Core.Cqrs
{
    public class EventDbContext : DbContext
    {
        public DbSet<DomainEvent> DomainEvents { get; set; }

        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
        {

        }
    }

    public class EventDbContextBuilder : IDesignTimeDbContextFactory<EventDbContext>
    {
        public EventDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<EventDbContext>();
            options.UseSqlite("Data Source=events.db");

            return new EventDbContext(options.Options); ;
        }
    }
}
