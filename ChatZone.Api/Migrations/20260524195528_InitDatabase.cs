using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatZone.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupChat",
                columns: table => new
                {
                    IdGroupChat = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Country = table.Column<int>(type: "int", nullable: true),
                    City = table.Column<int>(type: "int", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    Lang = table.Column<int>(type: "int", nullable: true),
                    UserCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupChat", x => x.IdGroupChat);
                });

            migrationBuilder.CreateTable(
                name: "MatchQueue",
                columns: table => new
                {
                    IdPerson = table.Column<int>(type: "int", nullable: false),
                    Theme = table.Column<int>(type: "int", nullable: true),
                    Country = table.Column<int>(type: "int", nullable: true),
                    City = table.Column<int>(type: "int", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    YourGender = table.Column<int>(type: "int", nullable: false),
                    PartnerGender = table.Column<int>(type: "int", nullable: false),
                    Language = table.Column<int>(type: "int", nullable: false),
                    ConnectionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JoinedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IsRandomPartner = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchQueue", x => x.IdPerson);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    IdPerson = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshTokenExp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LangMenu = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IsDarkTheme = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Theme = table.Column<int>(type: "int", nullable: true),
                    Country = table.Column<int>(type: "int", nullable: true),
                    City = table.Column<int>(type: "int", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: true),
                    YourGender = table.Column<int>(type: "int", nullable: true),
                    PartnerGender = table.Column<int>(type: "int", nullable: true),
                    Language = table.Column<int>(type: "int", nullable: true),
                    EmailConfirmToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmTokenExp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    PasswordResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordResetTokenExp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.IdPerson);
                });

            migrationBuilder.CreateTable(
                name: "Statistic",
                columns: table => new
                {
                    StatisticId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AllMember = table.Column<int>(type: "int", nullable: false),
                    OnlineNow = table.Column<int>(type: "int", nullable: false),
                    OnlineIndividual = table.Column<int>(type: "int", nullable: false),
                    OnlineGroup = table.Column<int>(type: "int", nullable: false),
                    OnlineToday = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistic", x => x.StatisticId);
                });

            migrationBuilder.CreateTable(
                name: "BlockedGroupMember",
                columns: table => new
                {
                    IdChat = table.Column<int>(type: "int", nullable: false),
                    IdBlockedPerson = table.Column<int>(type: "int", nullable: false),
                    BlockedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockedGroupMember", x => new { x.IdChat, x.IdBlockedPerson });
                    table.ForeignKey(
                        name: "FK_BlockedGroupMember_GroupChat_IdChat",
                        column: x => x.IdChat,
                        principalTable: "GroupChat",
                        principalColumn: "IdGroupChat",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlockedGroupMember_Person_IdBlockedPerson",
                        column: x => x.IdBlockedPerson,
                        principalTable: "Person",
                        principalColumn: "IdPerson",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlockedPerson",
                columns: table => new
                {
                    IdBlockerPerson = table.Column<int>(type: "int", nullable: false),
                    IdBlockedPerson = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockedPerson", x => new { x.IdBlockerPerson, x.IdBlockedPerson });
                    table.ForeignKey(
                        name: "FK_BlockedPerson_Person_IdBlockedPerson",
                        column: x => x.IdBlockedPerson,
                        principalTable: "Person",
                        principalColumn: "IdPerson",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlockedPerson_Person_IdBlockerPerson",
                        column: x => x.IdBlockerPerson,
                        principalTable: "Person",
                        principalColumn: "IdPerson",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GroupMember",
                columns: table => new
                {
                    IdChat = table.Column<int>(type: "int", nullable: false),
                    IdGroupMember = table.Column<int>(type: "int", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    JoinedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMember", x => new { x.IdChat, x.IdGroupMember });
                    table.ForeignKey(
                        name: "FK_GroupMember_GroupChat_IdChat",
                        column: x => x.IdChat,
                        principalTable: "GroupChat",
                        principalColumn: "IdGroupChat",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMember_Person_IdGroupMember",
                        column: x => x.IdGroupMember,
                        principalTable: "Person",
                        principalColumn: "IdPerson",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupMessage",
                columns: table => new
                {
                    IdGroupMessage = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    IdSender = table.Column<int>(type: "int", nullable: false),
                    IdChat = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMessage", x => x.IdGroupMessage);
                    table.ForeignKey(
                        name: "FK_GroupMessage_GroupChat_IdChat",
                        column: x => x.IdChat,
                        principalTable: "GroupChat",
                        principalColumn: "IdGroupChat",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMessage_Person_IdSender",
                        column: x => x.IdSender,
                        principalTable: "Person",
                        principalColumn: "IdPerson",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuickMessage",
                columns: table => new
                {
                    IdQuickMessage = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdPerson = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuickMessage", x => x.IdQuickMessage);
                    table.ForeignKey(
                        name: "FK_QuickMessage_Person_IdPerson",
                        column: x => x.IdPerson,
                        principalTable: "Person",
                        principalColumn: "IdPerson",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SingleChat",
                columns: table => new
                {
                    IdSingleChat = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdFirstPerson = table.Column<int>(type: "int", nullable: false),
                    IdSecondPerson = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FinishedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleChat", x => x.IdSingleChat);
                    table.ForeignKey(
                        name: "FK_SingleChat_Person_IdFirstPerson",
                        column: x => x.IdFirstPerson,
                        principalTable: "Person",
                        principalColumn: "IdPerson",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SingleChat_Person_IdSecondPerson",
                        column: x => x.IdSecondPerson,
                        principalTable: "Person",
                        principalColumn: "IdPerson",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SingleMessage",
                columns: table => new
                {
                    IdSingleMessage = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IdSender = table.Column<int>(type: "int", nullable: false),
                    IdChat = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleMessage", x => x.IdSingleMessage);
                    table.ForeignKey(
                        name: "FK_SingleMessage_Person_IdSender",
                        column: x => x.IdSender,
                        principalTable: "Person",
                        principalColumn: "IdPerson",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SingleMessage_SingleChat_IdChat",
                        column: x => x.IdChat,
                        principalTable: "SingleChat",
                        principalColumn: "IdSingleChat",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    IdReport = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportTheme = table.Column<int>(type: "int", nullable: false),
                    ReportMessage = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IdReporter = table.Column<int>(type: "int", nullable: false),
                    IdReported = table.Column<int>(type: "int", nullable: false),
                    IdSingleMessageReport = table.Column<int>(type: "int", nullable: true),
                    IdGroupMessageReport = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.IdReport);
                    table.ForeignKey(
                        name: "FK_Report_GroupMessage_IdGroupMessageReport",
                        column: x => x.IdGroupMessageReport,
                        principalTable: "GroupMessage",
                        principalColumn: "IdGroupMessage",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Report_Person_IdReported",
                        column: x => x.IdReported,
                        principalTable: "Person",
                        principalColumn: "IdPerson",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Report_Person_IdReporter",
                        column: x => x.IdReporter,
                        principalTable: "Person",
                        principalColumn: "IdPerson",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Report_SingleMessage_IdSingleMessageReport",
                        column: x => x.IdSingleMessageReport,
                        principalTable: "SingleMessage",
                        principalColumn: "IdSingleMessage",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlockedGroupMember_IdBlockedPerson",
                table: "BlockedGroupMember",
                column: "IdBlockedPerson");

            migrationBuilder.CreateIndex(
                name: "IX_BlockedPerson_IdBlockedPerson",
                table: "BlockedPerson",
                column: "IdBlockedPerson");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_IdGroupMember",
                table: "GroupMember",
                column: "IdGroupMember",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessage_IdChat",
                table: "GroupMessage",
                column: "IdChat");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMessage_IdSender",
                table: "GroupMessage",
                column: "IdSender");

            migrationBuilder.CreateIndex(
                name: "IX_Person_Email",
                table: "Person",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Person_Username",
                table: "Person",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuickMessage_IdPerson",
                table: "QuickMessage",
                column: "IdPerson");

            migrationBuilder.CreateIndex(
                name: "IX_Report_IdGroupMessageReport",
                table: "Report",
                column: "IdGroupMessageReport",
                unique: true,
                filter: "[IdGroupMessageReport] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Report_IdReported",
                table: "Report",
                column: "IdReported");

            migrationBuilder.CreateIndex(
                name: "IX_Report_IdReporter",
                table: "Report",
                column: "IdReporter");

            migrationBuilder.CreateIndex(
                name: "IX_Report_IdSingleMessageReport",
                table: "Report",
                column: "IdSingleMessageReport",
                unique: true,
                filter: "[IdSingleMessageReport] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SingleChat_IdFirstPerson",
                table: "SingleChat",
                column: "IdFirstPerson");

            migrationBuilder.CreateIndex(
                name: "IX_SingleChat_IdSecondPerson",
                table: "SingleChat",
                column: "IdSecondPerson");

            migrationBuilder.CreateIndex(
                name: "IX_SingleMessage_IdChat",
                table: "SingleMessage",
                column: "IdChat");

            migrationBuilder.CreateIndex(
                name: "IX_SingleMessage_IdSender",
                table: "SingleMessage",
                column: "IdSender");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlockedGroupMember");

            migrationBuilder.DropTable(
                name: "BlockedPerson");

            migrationBuilder.DropTable(
                name: "GroupMember");

            migrationBuilder.DropTable(
                name: "MatchQueue");

            migrationBuilder.DropTable(
                name: "QuickMessage");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "Statistic");

            migrationBuilder.DropTable(
                name: "GroupMessage");

            migrationBuilder.DropTable(
                name: "SingleMessage");

            migrationBuilder.DropTable(
                name: "GroupChat");

            migrationBuilder.DropTable(
                name: "SingleChat");

            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
