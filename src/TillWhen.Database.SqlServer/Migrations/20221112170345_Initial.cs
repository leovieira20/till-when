using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TillWhen.Database.SqlServer.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkableQueues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkableQueues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkableBase",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Estimation_OriginalDuration = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    WorkableQueueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    workable_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkableBase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkableBase_WorkableQueues_WorkableQueueId",
                        column: x => x.WorkableQueueId,
                        principalTable: "WorkableQueues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkableQueueConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Capacity_OriginalDuration = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    WorkableQueueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkableQueueConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkableQueueConfigurations_WorkableQueues_WorkableQueueId",
                        column: x => x.WorkableQueueId,
                        principalTable: "WorkableQueues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkableBase_WorkableQueueId",
                table: "WorkableBase",
                column: "WorkableQueueId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkableQueueConfigurations_WorkableQueueId",
                table: "WorkableQueueConfigurations",
                column: "WorkableQueueId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkableBase");

            migrationBuilder.DropTable(
                name: "WorkableQueueConfigurations");

            migrationBuilder.DropTable(
                name: "WorkableQueues");
        }
    }
}
