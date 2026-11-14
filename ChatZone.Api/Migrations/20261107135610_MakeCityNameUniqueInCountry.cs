using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatZone.Migrations
{
    /// <inheritdoc />
    public partial class MakeCityNameUniqueInCountry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_City_IdCountry_Name",
                table: "City");

            migrationBuilder.CreateIndex(
                name: "IX_City_IdCountry_Name",
                table: "City",
                columns: new[] { "IdCountry", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_City_IdCountry_Name",
                table: "City");

            migrationBuilder.CreateIndex(
                name: "IX_City_IdCountry_Name",
                table: "City",
                columns: new[] { "IdCountry", "Name" });
        }
    }
}
