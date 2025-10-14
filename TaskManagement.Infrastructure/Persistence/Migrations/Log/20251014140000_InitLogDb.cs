using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Infrastructure.Persistence.Migrations.Log
{
    /// <inheritdoc />
    public partial class InitLogDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntityLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityType = table.Column<byte>(type: "tinyint", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<byte>(type: "tinyint", nullable: false),
                    LogDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(6)", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntityRelationLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrimaryEntityType = table.Column<byte>(type: "tinyint", nullable: false),
                    PrimaryEntityId = table.Column<int>(type: "int", nullable: false),
                    SecondaryEntityType = table.Column<byte>(type: "tinyint", nullable: false),
                    SecondaryEntityId = table.Column<int>(type: "int", nullable: false),
                    ActorUserId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<byte>(type: "tinyint", nullable: false),
                    LogDescription = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2(6)", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityRelationLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityLogs_EntityId",
                table: "EntityLogs",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityRelationLogs_PrimaryEntityId_SecondaryEntityId_PrimaryEntityType_SecondaryEntityType",
                table: "EntityRelationLogs",
                columns: new[] { "PrimaryEntityId", "SecondaryEntityId", "PrimaryEntityType", "SecondaryEntityType" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityLogs");

            migrationBuilder.DropTable(
                name: "EntityRelationLogs");
        }
    }
}
