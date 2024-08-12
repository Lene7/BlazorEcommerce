﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorEccomerce.Server.Migrations
{
    public partial class productidchanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CartItemId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CartItemId",
                table: "Products",
                column: "CartItemId");


            migrationBuilder.AddForeignKey(
                name: "FK_Products_CartItems_CartItemId",
                table: "Products",
                column: "CartItemId",
                principalTable: "CartItems",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_CartItems_CartItemId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CartItemId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "CartItemId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "CartItems");
        }
    }
}
