using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Inventory.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryMasters",
                columns: table => new
                {
                    AggregateRootId = table.Column<Guid>(nullable: false),
                    GeneralNomenclature = table.Column<string>(nullable: true),
                    Id = table.Column<int>(nullable: false),
                    IsGArmy = table.Column<bool>(nullable: false),
                    LIN = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    TrackingType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryMasters", x => x.AggregateRootId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryMasters");
        }
    }
}
