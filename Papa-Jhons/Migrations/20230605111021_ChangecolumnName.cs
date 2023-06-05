using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Papa_Jhons.Migrations
{
    public partial class ChangecolumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Adress",
                table: "Orders",
                newName: "Address");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Orders",
                newName: "Adress");
        }
    }
}
