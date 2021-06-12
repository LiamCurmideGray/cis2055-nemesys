using Microsoft.EntityFrameworkCore.Migrations;

namespace cis2055_NemesysProject.Migrations
{
    public partial class Removed_ReportHazards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportHazard");

            migrationBuilder.AddColumn<int>(
                name: "Hazard_ID",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_Hazard_ID",
                table: "Reports",
                column: "Hazard_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Hazards",
                table: "Reports",
                column: "Hazard_ID",
                principalTable: "Hazards",
                principalColumn: "Hazard_ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Hazards",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_Hazard_ID",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Hazard_ID",
                table: "Reports");

            migrationBuilder.CreateTable(
                name: "ReportHazard",
                columns: table => new
                {
                    Hazard_ID = table.Column<int>(type: "int", nullable: false),
                    Report_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportHazard", x => new { x.Hazard_ID, x.Report_ID });
                    table.ForeignKey(
                        name: "FK_ReportHazard_Hazard",
                        column: x => x.Hazard_ID,
                        principalTable: "Hazards",
                        principalColumn: "Hazard_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportHazard_Reports",
                        column: x => x.Report_ID,
                        principalTable: "Reports",
                        principalColumn: "Report_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportHazard_Report_ID",
                table: "ReportHazard",
                column: "Report_ID");
        }
    }
}
