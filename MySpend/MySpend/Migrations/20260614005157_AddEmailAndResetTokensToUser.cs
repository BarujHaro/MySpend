using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySpend.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailAndResetTokensToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResetTokenExpiration",
                table: "Users",
                newName: "ResetTokenExpiresAt");

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "EmailToken",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EmailTokenExpiresAt",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailTokenExpiresAt",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ResetTokenExpiresAt",
                table: "Users",
                newName: "ResetTokenExpiration");
        }
    }
}
