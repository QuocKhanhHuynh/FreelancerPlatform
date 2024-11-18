using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerPlatform.Infratructure.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataModeV11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "hop_dong",
                columns: table => new
                {
                    ma_hop_dong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nguoi_tao = table.Column<int>(type: "int", nullable: false),
                    ten_hop_dong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    noi_dung_hop_dong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    doi_tac = table.Column<int>(type: "int", nullable: false),
                    trang_thai_chap_nhan = table.Column<bool>(type: "bit", nullable: false),
                    trang_thai_hop_dong = table.Column<bool>(type: "bit", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hop_dong", x => x.ma_hop_dong);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hop_dong");
        }
    }
}
