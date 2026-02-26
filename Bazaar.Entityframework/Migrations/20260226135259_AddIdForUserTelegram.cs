using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bazaar.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class AddIdForUserTelegram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TelegramUserStates",
                table: "TelegramUserStates");

            migrationBuilder.AlterColumn<long>(
                name: "ChatId",
                table: "TelegramUserStates",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TelegramUserStates",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TelegramUserStates",
                table: "TelegramUserStates",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TelegramUserStates",
                table: "TelegramUserStates");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TelegramUserStates");

            migrationBuilder.AlterColumn<long>(
                name: "ChatId",
                table: "TelegramUserStates",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TelegramUserStates",
                table: "TelegramUserStates",
                column: "ChatId");
        }
    }
}
