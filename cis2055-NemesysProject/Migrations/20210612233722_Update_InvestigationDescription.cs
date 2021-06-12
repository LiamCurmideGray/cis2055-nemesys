using Microsoft.EntityFrameworkCore.Migrations;

namespace cis2055_NemesysProject.Migrations
{
    public partial class Update_InvestigationDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Investigations",
                type: "nvarchar(max)",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Investigations");
        }
    }
}
