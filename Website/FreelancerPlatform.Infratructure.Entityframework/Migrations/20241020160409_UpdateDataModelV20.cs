using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerPlatform.Infratructure.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataModelV20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bai_dang",
                columns: table => new
                {
                    ma_bai_dang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten_bai_dang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    noi_dung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    luot_thich = table.Column<int>(type: "int", nullable: false),
                    trang_thai_duyet = table.Column<bool>(type: "bit", nullable: false),
                    duong_dan_anh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FreelancerId = table.Column<int>(type: "int", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bai_dang", x => x.ma_bai_dang);
                    table.ForeignKey(
                        name: "FK_bai_dang_ung_vien_FreelancerId",
                        column: x => x.FreelancerId,
                        principalTable: "ung_vien",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "binh_luan",
                columns: table => new
                {
                    ma_binh_luan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    noi_dung_binh_luan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    luot_thich = table.Column<int>(type: "int", nullable: false),
                    Parent = table.Column<int>(type: "int", nullable: true),
                    Reply = table.Column<int>(type: "int", nullable: true),
                    FreelancerId = table.Column<int>(type: "int", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_binh_luan", x => x.ma_binh_luan);
                    table.ForeignKey(
                        name: "FK_binh_luan_ung_vien_FreelancerId",
                        column: x => x.FreelancerId,
                        principalTable: "ung_vien",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_bai_dang_FreelancerId",
                table: "bai_dang",
                column: "FreelancerId");

            migrationBuilder.CreateIndex(
                name: "IX_binh_luan_FreelancerId",
                table: "binh_luan",
                column: "FreelancerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bai_dang");

            migrationBuilder.DropTable(
                name: "binh_luan");
        }
    }
}
