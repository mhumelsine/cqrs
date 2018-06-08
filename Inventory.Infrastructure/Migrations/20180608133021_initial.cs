using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Infrastructure.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DomainEvents",
                columns: table => new
                {
                    EventId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventName = table.Column<string>(nullable: true),
                    EventTimestamp = table.Column<DateTime>(nullable: false),
                    AggregateRootId = table.Column<Guid>(nullable: false),
                    UserCreated = table.Column<string>(nullable: true),
                    EventSequence = table.Column<int>(nullable: false),
                    EventData = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainEvents", x => x.EventId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DomainEvents");
        }
    }
}
