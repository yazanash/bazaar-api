using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bazaar.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class RenameBackstorageLengthColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BackstorageLenght",
                table: "TruckSpecs",
                newName: "BackstorageLength");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BackstorageLength",
                table: "TruckSpecs",
                newName: "BackstorageLenght");
        }
    }
}
