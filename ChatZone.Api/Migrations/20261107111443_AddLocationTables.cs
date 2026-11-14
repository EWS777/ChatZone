using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatZone.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MatchQueue_IsRandomPartner_Language",
                table: "MatchQueue");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "MatchQueue");

            migrationBuilder.RenameColumn(
                name: "Language",
                table: "SingleChat",
                newName: "IdLanguage");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "SingleChat",
                newName: "IdCountry");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "SingleChat",
                newName: "IdCity");

            migrationBuilder.RenameColumn(
                name: "Language",
                table: "Person",
                newName: "IdLanguage");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Person",
                newName: "IdCountry");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Person",
                newName: "IdCity");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "MatchQueue",
                newName: "IdLanguage");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "MatchQueue",
                newName: "IdCountry");

            migrationBuilder.RenameColumn(
                name: "Lang",
                table: "GroupChat",
                newName: "IdLanguage");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "GroupChat",
                newName: "IdCountry");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "GroupChat",
                newName: "IdCity");

            // old values were enum numbers that do not match the new Country/City/Language ids (test data)
            migrationBuilder.Sql("UPDATE [Person] SET [IdCountry] = NULL, [IdCity] = NULL, [IdLanguage] = NULL");
            migrationBuilder.Sql("UPDATE [SingleChat] SET [IdCountry] = NULL, [IdCity] = NULL, [IdLanguage] = NULL");
            migrationBuilder.Sql("UPDATE [GroupChat] SET [IdCountry] = NULL, [IdCity] = NULL, [IdLanguage] = NULL");
            migrationBuilder.Sql("DELETE FROM [MatchQueue]");

            migrationBuilder.AddColumn<int>(
                name: "IdCity",
                table: "MatchQueue",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    IdCountry = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsoCode = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.IdCountry);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    IdLanguage = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsoCode = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.IdLanguage);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    IdCity = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCountry = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Population = table.Column<int>(type: "int", nullable: false),
                    GeoNameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.IdCity);
                    table.ForeignKey(
                        name: "FK_City_Country_IdCountry",
                        column: x => x.IdCountry,
                        principalTable: "Country",
                        principalColumn: "IdCountry",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SingleChat_IdCity",
                table: "SingleChat",
                column: "IdCity");

            migrationBuilder.CreateIndex(
                name: "IX_SingleChat_IdCountry",
                table: "SingleChat",
                column: "IdCountry");

            migrationBuilder.CreateIndex(
                name: "IX_SingleChat_IdLanguage",
                table: "SingleChat",
                column: "IdLanguage");

            migrationBuilder.CreateIndex(
                name: "IX_Person_IdCity",
                table: "Person",
                column: "IdCity");

            migrationBuilder.CreateIndex(
                name: "IX_Person_IdCountry",
                table: "Person",
                column: "IdCountry");

            migrationBuilder.CreateIndex(
                name: "IX_Person_IdLanguage",
                table: "Person",
                column: "IdLanguage");

            migrationBuilder.CreateIndex(
                name: "IX_MatchQueue_IsRandomPartner_IdLanguage",
                table: "MatchQueue",
                columns: new[] { "IsRandomPartner", "IdLanguage" });

            migrationBuilder.CreateIndex(
                name: "IX_GroupChat_IdCity",
                table: "GroupChat",
                column: "IdCity");

            migrationBuilder.CreateIndex(
                name: "IX_GroupChat_IdCountry",
                table: "GroupChat",
                column: "IdCountry");

            migrationBuilder.CreateIndex(
                name: "IX_GroupChat_IdLanguage",
                table: "GroupChat",
                column: "IdLanguage");

            migrationBuilder.CreateIndex(
                name: "IX_City_GeoNameId",
                table: "City",
                column: "GeoNameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_City_IdCountry_Name",
                table: "City",
                columns: new[] { "IdCountry", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Country_IsoCode",
                table: "Country",
                column: "IsoCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Language_IsoCode",
                table: "Language",
                column: "IsoCode",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChat_City_IdCity",
                table: "GroupChat",
                column: "IdCity",
                principalTable: "City",
                principalColumn: "IdCity",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChat_Country_IdCountry",
                table: "GroupChat",
                column: "IdCountry",
                principalTable: "Country",
                principalColumn: "IdCountry",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChat_Language_IdLanguage",
                table: "GroupChat",
                column: "IdLanguage",
                principalTable: "Language",
                principalColumn: "IdLanguage",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Person_City_IdCity",
                table: "Person",
                column: "IdCity",
                principalTable: "City",
                principalColumn: "IdCity",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Country_IdCountry",
                table: "Person",
                column: "IdCountry",
                principalTable: "Country",
                principalColumn: "IdCountry",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Language_IdLanguage",
                table: "Person",
                column: "IdLanguage",
                principalTable: "Language",
                principalColumn: "IdLanguage",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SingleChat_City_IdCity",
                table: "SingleChat",
                column: "IdCity",
                principalTable: "City",
                principalColumn: "IdCity",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SingleChat_Country_IdCountry",
                table: "SingleChat",
                column: "IdCountry",
                principalTable: "Country",
                principalColumn: "IdCountry",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SingleChat_Language_IdLanguage",
                table: "SingleChat",
                column: "IdLanguage",
                principalTable: "Language",
                principalColumn: "IdLanguage",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupChat_City_IdCity",
                table: "GroupChat");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupChat_Country_IdCountry",
                table: "GroupChat");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupChat_Language_IdLanguage",
                table: "GroupChat");

            migrationBuilder.DropForeignKey(
                name: "FK_Person_City_IdCity",
                table: "Person");

            migrationBuilder.DropForeignKey(
                name: "FK_Person_Country_IdCountry",
                table: "Person");

            migrationBuilder.DropForeignKey(
                name: "FK_Person_Language_IdLanguage",
                table: "Person");

            migrationBuilder.DropForeignKey(
                name: "FK_SingleChat_City_IdCity",
                table: "SingleChat");

            migrationBuilder.DropForeignKey(
                name: "FK_SingleChat_Country_IdCountry",
                table: "SingleChat");

            migrationBuilder.DropForeignKey(
                name: "FK_SingleChat_Language_IdLanguage",
                table: "SingleChat");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropIndex(
                name: "IX_SingleChat_IdCity",
                table: "SingleChat");

            migrationBuilder.DropIndex(
                name: "IX_SingleChat_IdCountry",
                table: "SingleChat");

            migrationBuilder.DropIndex(
                name: "IX_SingleChat_IdLanguage",
                table: "SingleChat");

            migrationBuilder.DropIndex(
                name: "IX_Person_IdCity",
                table: "Person");

            migrationBuilder.DropIndex(
                name: "IX_Person_IdCountry",
                table: "Person");

            migrationBuilder.DropIndex(
                name: "IX_Person_IdLanguage",
                table: "Person");

            migrationBuilder.DropIndex(
                name: "IX_MatchQueue_IsRandomPartner_IdLanguage",
                table: "MatchQueue");

            migrationBuilder.DropIndex(
                name: "IX_GroupChat_IdCity",
                table: "GroupChat");

            migrationBuilder.DropIndex(
                name: "IX_GroupChat_IdCountry",
                table: "GroupChat");

            migrationBuilder.DropIndex(
                name: "IX_GroupChat_IdLanguage",
                table: "GroupChat");

            migrationBuilder.DropColumn(
                name: "IdCity",
                table: "MatchQueue");

            migrationBuilder.RenameColumn(
                name: "IdLanguage",
                table: "SingleChat",
                newName: "Language");

            migrationBuilder.RenameColumn(
                name: "IdCountry",
                table: "SingleChat",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "IdCity",
                table: "SingleChat",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "IdLanguage",
                table: "Person",
                newName: "Language");

            migrationBuilder.RenameColumn(
                name: "IdCountry",
                table: "Person",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "IdCity",
                table: "Person",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "IdLanguage",
                table: "MatchQueue",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "IdCountry",
                table: "MatchQueue",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "IdLanguage",
                table: "GroupChat",
                newName: "Lang");

            migrationBuilder.RenameColumn(
                name: "IdCountry",
                table: "GroupChat",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "IdCity",
                table: "GroupChat",
                newName: "City");

            migrationBuilder.AddColumn<int>(
                name: "Language",
                table: "MatchQueue",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MatchQueue_IsRandomPartner_Language",
                table: "MatchQueue",
                columns: new[] { "IsRandomPartner", "Language" });
        }
    }
}
