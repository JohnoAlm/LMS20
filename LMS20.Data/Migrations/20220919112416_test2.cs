using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMS20.Data.Migrations
{
    public partial class test2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "ModuleActivity");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "StartDateTime",
                table: "Modules",
                newName: "Start");

            migrationBuilder.RenameColumn(
                name: "StartDateTime",
                table: "ModuleActivity",
                newName: "Start");

            migrationBuilder.RenameColumn(
                name: "StartDateTime",
                table: "Courses",
                newName: "Start");

            migrationBuilder.AddColumn<DateTime>(
                name: "End",
                table: "Modules",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "End",
                table: "ModuleActivity",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "End",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "End",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "End",
                table: "ModuleActivity");

            migrationBuilder.DropColumn(
                name: "End",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "Start",
                table: "Modules",
                newName: "StartDateTime");

            migrationBuilder.RenameColumn(
                name: "Start",
                table: "ModuleActivity",
                newName: "StartDateTime");

            migrationBuilder.RenameColumn(
                name: "Start",
                table: "Courses",
                newName: "StartDateTime");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Modules",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "ModuleActivity",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Courses",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
