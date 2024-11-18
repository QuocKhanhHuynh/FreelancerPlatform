using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerPlatform.Infratructure.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataModelV23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bai_dang_luu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    FreelancerId = table.Column<int>(type: "int", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bai_dang_luu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bai_dang_luu_bai_dang_PostId",
                        column: x => x.PostId,
                        principalTable: "bai_dang",
                        principalColumn: "ma_bai_dang");
                    table.ForeignKey(
                        name: "FK_bai_dang_luu_ung_vien_FreelancerId",
                        column: x => x.FreelancerId,
                        principalTable: "ung_vien",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_bai_dang_luu_FreelancerId",
                table: "bai_dang_luu",
                column: "FreelancerId");

            migrationBuilder.CreateIndex(
                name: "IX_bai_dang_luu_PostId",
                table: "bai_dang_luu",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bai_dang_luu");
        }
    }
}
