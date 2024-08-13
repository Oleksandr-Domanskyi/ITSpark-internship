using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenamesomeEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_ItemProfile_ItemProfileId",
                schema: "identity",
                table: "Image");

            migrationBuilder.DropTable(
                name: "ItemProfile",
                schema: "identity");

            migrationBuilder.RenameColumn(
                name: "ItemProfileId",
                schema: "identity",
                table: "Image",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Image_ItemProfileId",
                schema: "identity",
                table: "Image",
                newName: "IX_Image_ProductId");

            migrationBuilder.CreateTable(
                name: "Product",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Product_ProductId",
                schema: "identity",
                table: "Image",
                column: "ProductId",
                principalSchema: "identity",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Product_ProductId",
                schema: "identity",
                table: "Image");

            migrationBuilder.DropTable(
                name: "Product",
                schema: "identity");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                schema: "identity",
                table: "Image",
                newName: "ItemProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Image_ProductId",
                schema: "identity",
                table: "Image",
                newName: "IX_Image_ItemProfileId");

            migrationBuilder.CreateTable(
                name: "ItemProfile",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemProfile", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Image_ItemProfile_ItemProfileId",
                schema: "identity",
                table: "Image",
                column: "ItemProfileId",
                principalSchema: "identity",
                principalTable: "ItemProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
