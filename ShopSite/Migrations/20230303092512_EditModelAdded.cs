using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopSite.Migrations
{
    /// <inheritdoc />
    public partial class EditModelAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EditModels",
                columns: table => new
                {
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Product1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Product2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Product3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Product4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Product5 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Product6 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EditModels");
        }
    }
}
