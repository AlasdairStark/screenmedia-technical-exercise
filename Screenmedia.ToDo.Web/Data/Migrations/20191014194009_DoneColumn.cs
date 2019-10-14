using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Screenmedia.ToDo.Web.Data.Migrations
{
    public partial class DoneColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder == null)
                throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.DropForeignKey(
                name: "FK_ToDoNotes_AspNetUsers_ApplicationUserId1",
                table: "ToDoNotes");

            migrationBuilder.DropIndex(
                name: "IX_ToDoNotes_ApplicationUserId1",
                table: "ToDoNotes");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "ToDoNotes");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "ToDoNotes",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "Done",
                table: "ToDoNotes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ToDoNotes_ApplicationUserId",
                table: "ToDoNotes",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoNotes_AspNetUsers_ApplicationUserId",
                table: "ToDoNotes",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder == null)
                throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.DropForeignKey(
                name: "FK_ToDoNotes_AspNetUsers_ApplicationUserId",
                table: "ToDoNotes");

            migrationBuilder.DropIndex(
                name: "IX_ToDoNotes_ApplicationUserId",
                table: "ToDoNotes");

            migrationBuilder.DropColumn(
                name: "Done",
                table: "ToDoNotes");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "ToDoNotes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "ToDoNotes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoNotes_ApplicationUserId1",
                table: "ToDoNotes",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoNotes_AspNetUsers_ApplicationUserId1",
                table: "ToDoNotes",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
