using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatZone.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SingleMessage_IdChat",
                table: "SingleMessage");

            migrationBuilder.DropIndex(
                name: "IX_SingleChat_IdFirstPerson",
                table: "SingleChat");

            migrationBuilder.DropIndex(
                name: "IX_SingleChat_IdSecondPerson",
                table: "SingleChat");

            migrationBuilder.DropIndex(
                name: "IX_QuickMessage_IdPerson",
                table: "QuickMessage");

            migrationBuilder.DropIndex(
                name: "IX_GroupMessage_IdChat",
                table: "GroupMessage");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "Person",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EmailConfirmToken",
                table: "Person",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SingleMessage_IdChat_CreatedAt",
                table: "SingleMessage",
                columns: new[] { "IdChat", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_SingleChat_IdFirstPerson",
                table: "SingleChat",
                column: "IdFirstPerson",
                unique: true,
                filter: "[FinishedAt] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SingleChat_IdFirstPerson_FinishedAt",
                table: "SingleChat",
                columns: new[] { "IdFirstPerson", "FinishedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_SingleChat_IdSecondPerson",
                table: "SingleChat",
                column: "IdSecondPerson",
                unique: true,
                filter: "[FinishedAt] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SingleChat_IdSecondPerson_FinishedAt",
                table: "SingleChat",
                columns: new[] { "IdSecondPerson", "FinishedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_QuickMessage_IdPerson",
                table: "QuickMessage",
                column: "IdPerson")
                .Annotation("SqlServer:Include", new[] { "Message" });

            migrationBuilder.CreateIndex(
                name: "IX_Person_EmailConfirmToken",
                table: "Person",
                column: "EmailConfirmToken",
                unique: true,
                filter: "[EmailConfirmToken] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Person_RefreshToken",
                table: "Person",
                column: "RefreshToken",
                unique: true,
                filter: "[RefreshToken] <> ''");

            migrationBuilder.CreateIndex(
                name: "IX_MatchQueue_IsRandomPartner_Language",
                table: "MatchQueue",
                columns: new[] { "IsRandomPartner", "Language" });

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessage_IdChat_CreatedAt",
                table: "GroupMessage",
                columns: new[] { "IdChat", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_GroupChat_CreatedAt_IdGroupChat",
                table: "GroupChat",
                columns: new[] { "CreatedAt", "IdGroupChat" });

            migrationBuilder.CreateIndex(
                name: "IX_BlockedPerson_IdBlockerPerson_CreatedAt",
                table: "BlockedPerson",
                columns: new[] { "IdBlockerPerson", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SingleMessage_IdChat_CreatedAt",
                table: "SingleMessage");

            migrationBuilder.DropIndex(
                name: "IX_SingleChat_IdFirstPerson",
                table: "SingleChat");

            migrationBuilder.DropIndex(
                name: "IX_SingleChat_IdFirstPerson_FinishedAt",
                table: "SingleChat");

            migrationBuilder.DropIndex(
                name: "IX_SingleChat_IdSecondPerson",
                table: "SingleChat");

            migrationBuilder.DropIndex(
                name: "IX_SingleChat_IdSecondPerson_FinishedAt",
                table: "SingleChat");

            migrationBuilder.DropIndex(
                name: "IX_QuickMessage_IdPerson",
                table: "QuickMessage");

            migrationBuilder.DropIndex(
                name: "IX_Person_EmailConfirmToken",
                table: "Person");

            migrationBuilder.DropIndex(
                name: "IX_Person_RefreshToken",
                table: "Person");

            migrationBuilder.DropIndex(
                name: "IX_MatchQueue_IsRandomPartner_Language",
                table: "MatchQueue");

            migrationBuilder.DropIndex(
                name: "IX_GroupMessage_IdChat_CreatedAt",
                table: "GroupMessage");

            migrationBuilder.DropIndex(
                name: "IX_GroupChat_CreatedAt_IdGroupChat",
                table: "GroupChat");

            migrationBuilder.DropIndex(
                name: "IX_BlockedPerson_IdBlockerPerson_CreatedAt",
                table: "BlockedPerson");

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "Person",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "EmailConfirmToken",
                table: "Person",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SingleMessage_IdChat",
                table: "SingleMessage",
                column: "IdChat");

            migrationBuilder.CreateIndex(
                name: "IX_SingleChat_IdFirstPerson",
                table: "SingleChat",
                column: "IdFirstPerson");

            migrationBuilder.CreateIndex(
                name: "IX_SingleChat_IdSecondPerson",
                table: "SingleChat",
                column: "IdSecondPerson");

            migrationBuilder.CreateIndex(
                name: "IX_QuickMessage_IdPerson",
                table: "QuickMessage",
                column: "IdPerson");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessage_IdChat",
                table: "GroupMessage",
                column: "IdChat");
        }
    }
}
