using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerPlatform.Infratructure.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataModelV16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "thong_bao",
                columns: table => new
                {
                    ma_thong_bao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    noi_dung_thong_bao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    trang_thai_da_xem = table.Column<bool>(type: "bit", nullable: false),
                    FreelancerId = table.Column<int>(type: "int", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_thong_bao", x => x.ma_thong_bao);
                    table.ForeignKey(
                        name: "FK_thong_bao_ung_vien_FreelancerId",
                        column: x => x.FreelancerId,
                        principalTable: "ung_vien",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_thong_bao_FreelancerId",
                table: "thong_bao",
                column: "FreelancerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "thong_bao");
        }
    }
}
