using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Papa_Jhons.Migrations
{
    public partial class addRulesTablechangeColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CV",
                table: "Rules");

            migrationBuilder.AddColumn<string>(
                name: "Rule",
                table: "Rules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rule",
                table: "Rules");

            migrationBuilder.AddColumn<bool>(
                name: "CV",
                table: "Rules",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
