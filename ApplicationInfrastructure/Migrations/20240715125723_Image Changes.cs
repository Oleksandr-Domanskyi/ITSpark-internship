﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImageChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_ItemProfile_ItemProfileId",
                schema: "identity",
                table: "Image");

            migrationBuilder.AlterColumn<Guid>(
                name: "ItemProfileId",
                schema: "identity",
                table: "Image",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_ItemProfile_ItemProfileId",
                schema: "identity",
                table: "Image");

            migrationBuilder.AlterColumn<Guid>(
                name: "ItemProfileId",
                schema: "identity",
                table: "Image",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_ItemProfile_ItemProfileId",
                schema: "identity",
                table: "Image",
                column: "ItemProfileId",
                principalSchema: "identity",
                principalTable: "ItemProfile",
                principalColumn: "Id");
        }
    }
}
