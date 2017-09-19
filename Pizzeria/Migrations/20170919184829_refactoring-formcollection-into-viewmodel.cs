using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Pizzeria.Migrations
{
    public partial class refactoringformcollectionintoviewmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enabled",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "BasketItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Enabled",
                table: "Ingredients",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "BasketItems",
                nullable: false,
                defaultValue: 0);
        }
    }
}
