using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SomeChangesOFDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_ItemProfiles_ItemProfileId",
                table: "Images");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemProfiles",
                table: "ItemProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Images",
                table: "Images");

            migrationBuilder.RenameTable(
                name: "ItemProfiles",
                newName: "ItemProfile");

            migrationBuilder.RenameTable(
                name: "Images",
                newName: "Image");

            migrationBuilder.RenameIndex(
                name: "IX_Images_ItemProfileId",
                table: "Image",
                newName: "IX_Image_ItemProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemProfile",
                table: "ItemProfile",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Image",
                table: "Image",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_ItemProfile_ItemProfileId",
                table: "Image",
                column: "ItemProfileId",
                principalTable: "ItemProfile",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_ItemProfile_ItemProfileId",
                table: "Image");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemProfile",
                table: "ItemProfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Image",
                table: "Image");

            migrationBuilder.RenameTable(
                name: "ItemProfile",
                newName: "ItemProfiles");

            migrationBuilder.RenameTable(
                name: "Image",
                newName: "Images");

            migrationBuilder.RenameIndex(
                name: "IX_Image_ItemProfileId",
                table: "Images",
                newName: "IX_Images_ItemProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemProfiles",
                table: "ItemProfiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Images",
                table: "Images",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_ItemProfiles_ItemProfileId",
                table: "Images",
                column: "ItemProfileId",
                principalTable: "ItemProfiles",
                principalColumn: "Id");
        }
    }
}
