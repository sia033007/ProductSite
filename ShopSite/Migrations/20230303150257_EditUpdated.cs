using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopSite.Migrations
{
    /// <inheritdoc />
    public partial class EditUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Product1",
                table: "EditModels");

            migrationBuilder.DropColumn(
                name: "Product2",
                table: "EditModels");

            migrationBuilder.DropColumn(
                name: "Product3",
                table: "EditModels");

            migrationBuilder.DropColumn(
                name: "Product4",
                table: "EditModels");

            migrationBuilder.DropColumn(
                name: "Product5",
                table: "EditModels");

            migrationBuilder.DropColumn(
                name: "Product6",
                table: "EditModels");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Product1",
                table: "EditModels",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Product2",
                table: "EditModels",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Product3",
                table: "EditModels",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Product4",
                table: "EditModels",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Product5",
                table: "EditModels",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Product6",
                table: "EditModels",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
