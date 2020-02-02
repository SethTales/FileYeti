using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FileYetiServer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransferJobs",
                columns: table => new
                {
                    JobId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    JobGuid = table.Column<Guid>(nullable: false),
                    TotalChunks = table.Column<int>(nullable: false),
                    TotalChunksReceived = table.Column<int>(nullable: false),
                    ChunkSizeBytes = table.Column<int>(nullable: false),
                    LastChunkRecieved = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferJobs", x => x.JobId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransferJobs");
        }
    }
}
