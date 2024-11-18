using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerPlatform.Infratructure.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataModekV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentVerification",
                table: "ung_vien");

            migrationBuilder.DropColumn(
                name: "gioi_tinh",
                table: "ung_vien");

            migrationBuilder.DropColumn(
                name: "ngay_sinh",
                table: "ung_vien");

            migrationBuilder.DropColumn(
                name: "que_quan",
                table: "ung_vien");

            migrationBuilder.DropColumn(
                name: "so_dien_thoai",
                table: "ung_vien");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PaymentVerification",
                table: "ung_vien",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "gioi_tinh",
                table: "ung_vien",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ngay_sinh",
                table: "ung_vien",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "que_quan",
                table: "ung_vien",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "so_dien_thoai",
                table: "ung_vien",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
