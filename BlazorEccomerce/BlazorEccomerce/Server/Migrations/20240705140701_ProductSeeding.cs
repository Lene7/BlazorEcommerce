using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorEccomerce.Server.Migrations
{
    public partial class ProductSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { 1, "One (1) hour photoshoot. This include between 80 to a 100 edited photos. This can be for portfolio, formals, social media, or just for the fun of it!", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSjUgaoL9tabSf-Ig21XIdTawnh-BgUEbQZWjFisvLr9hf3-ekVFntcXFCAHAlKoV24M6Q", 1600.00m, "Personal Photoshoot" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { 2, "Two (2) hours photoshoot. This include between 100 to 150 edited photos. This can be for special occasions, social media, or just for the love you share!", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTsNk9UUKvmqxEh2t17_5jCYBWO4e4mWK2sfw&s", 2000.00m, "Couples Photoshoot" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Price", "Title" },
                values: new object[] { 3, "Three (3) hours photoshoot. This include between 200 to 300 edited photos. This can be for events, live performances, or social media!", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT4JQhSxjiWp9Q3ClqBHpGwnoQI_MvLlmDph4xpBJq3N-I9zmsb1e-DJgPfEyupgMWgr9U", 5000.00m, "Live Photography" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
