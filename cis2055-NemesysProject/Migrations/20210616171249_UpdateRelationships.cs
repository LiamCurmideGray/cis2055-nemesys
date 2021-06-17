using Microsoft.EntityFrameworkCore.Migrations;

namespace cis2055_NemesysProject.Migrations
{
    public partial class UpdateRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Investigations_Report_ID",
                table: "Investigations");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Reports",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Investigations_Report_ID",
                table: "Investigations",
                column: "Report_ID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Investigations_Report_ID",
                table: "Investigations");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Reports");

            migrationBuilder.CreateIndex(
                name: "IX_Investigations_Report_ID",
                table: "Investigations",
                column: "Report_ID");
        }
    }
}
