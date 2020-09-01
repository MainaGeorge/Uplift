using Microsoft.EntityFrameworkCore.Migrations;

namespace Uplift.DataAccess.Migrations
{
    public partial class AddingServiceCountToOrderHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceCount",
                table: "OrderHeader",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceCount",
                table: "OrderHeader");
        }
    }
}
