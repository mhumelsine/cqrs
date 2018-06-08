using Inventory.Inventory;
using Isf.Core.Cqrs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Infrastructure
{
    public class DomainDbContext : DbContext
    {
        public DbSet<InventoryMaster> InventoryMasters { get; set; }
        public DbSet<DomainEvent> DomainEvents { get; set; }

        public DomainDbContext(DbContextOptions<DomainDbContext> options) : base(options)
        {

        }
    }

    public class DomainDbContextFactory : IDesignTimeDbContextFactory<DomainDbContext>
    {
        public DomainDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<DomainDbContext>();
            options.UseSqlite("Data Source=domain.db");

            return new DomainDbContext(options.Options);
        }
    }
}
