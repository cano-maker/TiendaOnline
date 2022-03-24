using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiendaOnline.Web.Migrations
{
    public partial class new_field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "populate",
                table: "Countries",
                type: "int",
                maxLength: 10000,
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "populate",
                table: "Countries");
        }
    }
}
