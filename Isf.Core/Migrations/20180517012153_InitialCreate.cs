using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Isf.Core.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DomainEvents",
                columns: table => new
                {
                    EventId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AggregateRootId = table.Column<Guid>(nullable: false),
                    EventData = table.Column<string>(nullable: true),
                    EventName = table.Column<string>(nullable: true),
                    EventSequence = table.Column<int>(nullable: false),
                    EventTimestamp = table.Column<DateTime>(nullable: false),
                    UserCreated = table.Column<string>(nullable: true)
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
