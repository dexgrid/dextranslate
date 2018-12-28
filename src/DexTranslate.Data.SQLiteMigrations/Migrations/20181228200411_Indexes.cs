using Microsoft.EntityFrameworkCore.Migrations;

namespace DexTranslate.Data.SQLiteMigrations.Migrations
{
    public partial class Indexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Translations_LanguageKey_ProjectKey_Key",
                table: "Translations",
                columns: new[] { "LanguageKey", "ProjectKey", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Key",
                table: "Projects",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Key",
                table: "Languages",
                column: "Key",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Translations_LanguageKey_ProjectKey_Key",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Projects_Key",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Languages_Key",
                table: "Languages");
        }
    }
}
