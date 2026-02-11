using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bazaar.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class RemoveManfuctrerRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleAds_Manufacturers_ManufacturerId",
                table: "VehicleAds");

            migrationBuilder.DropIndex(
                name: "IX_VehicleAds_ManufacturerId",
                table: "VehicleAds");

            migrationBuilder.DropColumn(
                name: "ManufacturerId",
                table: "VehicleAds");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ManufacturerId",
                table: "VehicleAds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleAds_ManufacturerId",
                table: "VehicleAds",
                column: "ManufacturerId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleAds_Manufacturers_ManufacturerId",
                table: "VehicleAds",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
