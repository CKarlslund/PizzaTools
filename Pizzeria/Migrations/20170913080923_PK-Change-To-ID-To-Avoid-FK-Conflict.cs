using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Pizzeria.Migrations
{
    public partial class PKChangeToIDToAvoidFKConflict : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItemIngredients_BasketItems_BasketItemId",
                table: "BasketItemIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_BasketItemIngredients_Ingredients_IngredientId",
                table: "BasketItemIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_DishIngredients_Ingredients_IngredientId",
                table: "DishIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BasketItems",
                table: "BasketItems");

            migrationBuilder.DropColumn(
                name: "IngredientId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "BasketItemId",
                table: "BasketItems");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Ingredients",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BasketItems",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BasketItems",
                table: "BasketItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItemIngredients_BasketItems_BasketItemId",
                table: "BasketItemIngredients",
                column: "BasketItemId",
                principalTable: "BasketItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItemIngredients_Ingredients_IngredientId",
                table: "BasketItemIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DishIngredients_Ingredients_IngredientId",
                table: "DishIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItemIngredients_BasketItems_BasketItemId",
                table: "BasketItemIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_BasketItemIngredients_Ingredients_IngredientId",
                table: "BasketItemIngredients");

            migrationBuilder.DropForeignKey(
                name: "FK_DishIngredients_Ingredients_IngredientId",
                table: "DishIngredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BasketItems",
                table: "BasketItems");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BasketItems");

            migrationBuilder.AddColumn<int>(
                name: "IngredientId",
                table: "Ingredients",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "BasketItemId",
                table: "BasketItems",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingredients",
                table: "Ingredients",
                column: "IngredientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BasketItems",
                table: "BasketItems",
                column: "BasketItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItemIngredients_BasketItems_BasketItemId",
                table: "BasketItemIngredients",
                column: "BasketItemId",
                principalTable: "BasketItems",
                principalColumn: "BasketItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItemIngredients_Ingredients_IngredientId",
                table: "BasketItemIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DishIngredients_Ingredients_IngredientId",
                table: "DishIngredients",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
