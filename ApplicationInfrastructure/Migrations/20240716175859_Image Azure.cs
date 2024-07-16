using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImageAzure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataFile",
                schema: "identity",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "FileType",
                schema: "identity",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "ImageName",
                schema: "identity",
                table: "Image");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                schema: "identity",
                table: "Image",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                schema: "identity",
                table: "Image");

            migrationBuilder.AddColumn<byte[]>(
                name: "DataFile",
                schema: "identity",
                table: "Image",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileType",
                schema: "identity",
                table: "Image",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                schema: "identity",
                table: "Image",
                type: "text",
                nullable: true);
        }
    }
}
