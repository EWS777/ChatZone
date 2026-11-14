using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatZone.Migrations
{
    /// <inheritdoc />
    public partial class AddSingleChatSearchParams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "SingleChat",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "City",
                table: "SingleChat",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Country",
                table: "SingleChat",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Language",
                table: "SingleChat",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Theme",
                table: "SingleChat",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "SingleChat");

            migrationBuilder.DropColumn(
                name: "City",
                table: "SingleChat");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "SingleChat");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "SingleChat");

            migrationBuilder.DropColumn(
                name: "Theme",
                table: "SingleChat");
        }
    }
}
