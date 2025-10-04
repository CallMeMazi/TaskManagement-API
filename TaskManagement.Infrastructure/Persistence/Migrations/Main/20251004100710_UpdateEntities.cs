using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Infrastructure.Persistence.Migrations.Main
{
    /// <inheritdoc />
    public partial class UpdateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TaskInfoDescreption",
                table: "TaskInfos");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartedTaskAt",
                table: "TaskInfos",
                type: "datetime2(6)",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndedTaskAt",
                table: "TaskInfos",
                type: "datetime2(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(6)",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AddColumn<string>(
                name: "TaskInfoDescription",
                table: "TaskInfos",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "TotalHours",
                table: "TaskInfos",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "SecondOrgName",
                table: "Organizations",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskInfoDescription",
                table: "TaskInfos");

            migrationBuilder.DropColumn(
                name: "TotalHours",
                table: "TaskInfos");

            migrationBuilder.DropColumn(
                name: "SecondOrgName",
                table: "Organizations");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartedTaskAt",
                table: "TaskInfos",
                type: "datetime2(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(6)",
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndedTaskAt",
                table: "TaskInfos",
                type: "datetime2(6)",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2(6)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaskInfoDescreption",
                table: "TaskInfos",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }
    }
}
