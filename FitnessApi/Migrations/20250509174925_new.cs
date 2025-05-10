using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApi.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyStepActivitys_AspNetUsers_ApplicationUserId",
                table: "DailyStepActivitys");

            migrationBuilder.DropIndex(
                name: "IX_DailyStepActivitys_ApplicationUserId",
                table: "DailyStepActivitys");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "DailyStepActivitys");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "DailyStepActivitys",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_DailyStepActivitys_UserId",
                table: "DailyStepActivitys",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyStepActivitys_AspNetUsers_UserId",
                table: "DailyStepActivitys",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyStepActivitys_AspNetUsers_UserId",
                table: "DailyStepActivitys");

            migrationBuilder.DropIndex(
                name: "IX_DailyStepActivitys_UserId",
                table: "DailyStepActivitys");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "DailyStepActivitys",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "DailyStepActivitys",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_DailyStepActivitys_ApplicationUserId",
                table: "DailyStepActivitys",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyStepActivitys_AspNetUsers_ApplicationUserId",
                table: "DailyStepActivitys",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
