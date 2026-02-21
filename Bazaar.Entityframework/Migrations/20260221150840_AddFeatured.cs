using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bazaar.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class AddFeatured : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Special",
                table: "VehicleAds",
                newName: "Featured");

            migrationBuilder.AddColumn<DateTime>(
                name: "FeaturedUntil",
                table: "VehicleAds",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeaturedUntil",
                table: "VehicleAds");

            migrationBuilder.RenameColumn(
                name: "Featured",
                table: "VehicleAds",
                newName: "Special");
        }
    }
}
