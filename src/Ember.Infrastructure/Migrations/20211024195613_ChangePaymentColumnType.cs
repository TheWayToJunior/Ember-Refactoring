using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ember.Infrastructure.Migrations
{
    public partial class ChangePaymentColumnType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Payment",
                table: "PersonalAccounts",
                type: "Money",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2021, 10, 24, 22, 56, 12, 490, DateTimeKind.Local).AddTicks(6309));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Id",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2021, 11, 13, 22, 56, 12, 490, DateTimeKind.Local).AddTicks(6684));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Time",
                value: new DateTime(2021, 10, 24, 22, 56, 12, 487, DateTimeKind.Local).AddTicks(7290));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Time",
                value: new DateTime(2021, 10, 24, 22, 56, 12, 489, DateTimeKind.Local).AddTicks(944));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3,
                column: "Time",
                value: new DateTime(2021, 10, 24, 22, 56, 12, 489, DateTimeKind.Local).AddTicks(982));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Payment",
                table: "PersonalAccounts",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Money");

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Id",
                keyValue: 1,
                column: "Date",
                value: new DateTime(2021, 10, 23, 15, 56, 3, 120, DateTimeKind.Local).AddTicks(7038));

            migrationBuilder.UpdateData(
                table: "Payment",
                keyColumn: "Id",
                keyValue: 2,
                column: "Date",
                value: new DateTime(2021, 11, 12, 15, 56, 3, 120, DateTimeKind.Local).AddTicks(7399));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Time",
                value: new DateTime(2021, 10, 23, 15, 56, 3, 118, DateTimeKind.Local).AddTicks(996));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 2,
                column: "Time",
                value: new DateTime(2021, 10, 23, 15, 56, 3, 119, DateTimeKind.Local).AddTicks(3044));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: 3,
                column: "Time",
                value: new DateTime(2021, 10, 23, 15, 56, 3, 119, DateTimeKind.Local).AddTicks(3071));
        }
    }
}
