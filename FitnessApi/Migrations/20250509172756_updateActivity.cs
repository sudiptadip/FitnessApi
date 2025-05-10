using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApi.Migrations
{
    /// <inheritdoc />
    public partial class updateActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kcal",
                table: "DailyActivities");

            migrationBuilder.DropColumn(
                name: "Km",
                table: "DailyActivities");

            migrationBuilder.DropColumn(
                name: "Minutes",
                table: "DailyActivities");

            migrationBuilder.DropColumn(
                name: "Step",
                table: "DailyActivities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Kcal",
                table: "DailyActivities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Km",
                table: "DailyActivities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Minutes",
                table: "DailyActivities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Step",
                table: "DailyActivities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
