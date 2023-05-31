using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Papa_Jhons.Migrations
{
    public partial class Createpizza : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PizzaCategoryId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PizzaCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PizzaCategory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_PizzaCategoryId",
                table: "Products",
                column: "PizzaCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_PizzaCategory_PizzaCategoryId",
                table: "Products",
                column: "PizzaCategoryId",
                principalTable: "PizzaCategory",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_PizzaCategory_PizzaCategoryId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "PizzaCategory");

            migrationBuilder.DropIndex(
                name: "IX_Products_PizzaCategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PizzaCategoryId",
                table: "Products");
        }
    }
}
