using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorEccomerce.Server.Migrations
{
    public partial class adminDashProductFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "ProductVariants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Visible",
                table: "ProductVariants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Visible",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "ProductVariants",
                keyColumn: "ProductVariantId",
                keyValue: 1,
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariants",
                keyColumn: "ProductVariantId",
                keyValue: 2,
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariants",
                keyColumn: "ProductVariantId",
                keyValue: 3,
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariants",
                keyColumn: "ProductVariantId",
                keyValue: 4,
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariants",
                keyColumn: "ProductVariantId",
                keyValue: 5,
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariants",
                keyColumn: "ProductVariantId",
                keyValue: 6,
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariants",
                keyColumn: "ProductVariantId",
                keyValue: 7,
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "ProductVariants",
                keyColumn: "ProductVariantId",
                keyValue: 8,
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "Visible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                column: "Visible",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "Visible",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Visible",
                table: "Products");
        }
    }
}
