using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixingAuthorization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                schema: "identity",
                table: "ItemProfile",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Initials",
                schema: "identity",
                table: "AspNetUsers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(5)",
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "IdentityUser",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "text", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUser", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemProfile_CreatedById",
                schema: "identity",
                table: "ItemProfile",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemProfile_IdentityUser_CreatedById",
                schema: "identity",
                table: "ItemProfile",
                column: "CreatedById",
                principalSchema: "identity",
                principalTable: "IdentityUser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemProfile_IdentityUser_CreatedById",
                schema: "identity",
                table: "ItemProfile");

            migrationBuilder.DropTable(
                name: "IdentityUser",
                schema: "identity");

            migrationBuilder.DropIndex(
                name: "IX_ItemProfile_CreatedById",
                schema: "identity",
                table: "ItemProfile");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                schema: "identity",
                table: "ItemProfile");

            migrationBuilder.AlterColumn<string>(
                name: "Initials",
                schema: "identity",
                table: "AspNetUsers",
                type: "character varying(5)",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
