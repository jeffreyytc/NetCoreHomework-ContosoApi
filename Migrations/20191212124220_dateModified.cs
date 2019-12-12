using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContosoApi.Migrations
{
    public partial class dateModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Person",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            //migrationBuilder.AlterColumn<byte[]>(
            //    name: "RowVersion",
            //    table: "Department",
            //    rowVersion: true,
            //    nullable: true,
            //    oldClrType: typeof(byte[]),
            //    oldType: "rowversion",
            //    oldRowVersion: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Department",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Course",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            //migrationBuilder.CreateTable(
            //    name: "DepartmentInsertResults",
            //    columns: table => new
            //    {
            //        DepartmentId = table.Column<int>(nullable: false),
            //        RowVersion = table.Column<byte[]>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "DepartmentInsertResults");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Course");

            //migrationBuilder.AlterColumn<byte[]>(
            //    name: "RowVersion",
            //    table: "Department",
            //    type: "rowversion",
            //    rowVersion: true,
            //    nullable: false,
            //    oldClrType: typeof(byte[]),
            //    oldRowVersion: true,
            //    oldNullable: true);
        }
    }
}
