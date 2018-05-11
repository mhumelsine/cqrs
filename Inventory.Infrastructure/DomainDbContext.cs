using Inventory.Inventory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Infrastructure
{
    public class DomainDbContext : DbContext
    {
        DbSet<InventoryMaster> InventoryMasters { get; set; }

        public DomainDbContext(DbContextOptions<DomainDbContext> options) : base(options)
        {

        }
    }
}
