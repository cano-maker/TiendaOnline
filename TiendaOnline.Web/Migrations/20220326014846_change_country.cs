using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiendaOnline.Web.Migrations
{
    public partial class change_country : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "Countries",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Countries",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Countries_name",
                table: "Countries",
                newName: "IX_Countries_Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Countries",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Countries",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Countries_Name",
                table: "Countries",
                newName: "IX_Countries_name");
        }
    }
}
