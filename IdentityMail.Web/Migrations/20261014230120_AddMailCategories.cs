using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IdentityMail.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddMailCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MailCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MailMessageCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserMessageId = table.Column<int>(type: "int", nullable: false),
                    MailCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailMessageCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MailMessageCategories_MailCategories_MailCategoryId",
                        column: x => x.MailCategoryId,
                        principalTable: "MailCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MailMessageCategories_UserMessages_UserMessageId",
                        column: x => x.UserMessageId,
                        principalTable: "UserMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MailMessageCategories_MailCategoryId",
                table: "MailMessageCategories",
                column: "MailCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MailMessageCategories_UserId_UserMessageId_MailCategoryId",
                table: "MailMessageCategories",
                columns: new[] { "UserId", "UserMessageId", "MailCategoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MailMessageCategories_UserMessageId",
                table: "MailMessageCategories",
                column: "UserMessageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MailMessageCategories");

            migrationBuilder.DropTable(
                name: "MailCategories");
        }
    }
}
