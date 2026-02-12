using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bazaar.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class RemoveThumbnail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "VehicleAds");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "VehicleImage",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "VehicleImage");

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "VehicleAds",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
