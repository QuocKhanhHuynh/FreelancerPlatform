using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerPlatform.Infratructure.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataModelV33 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cong_viec_xem_gan_day",
                columns: table => new
                {
                    JobId = table.Column<int>(type: "int", nullable: false),
                    FreelancerId = table.Column<int>(type: "int", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cong_viec_xem_gan_day", x => new { x.FreelancerId, x.JobId });
                    table.ForeignKey(
                        name: "FK_cong_viec_xem_gan_day_cong_viec_JobId",
                        column: x => x.JobId,
                        principalTable: "cong_viec",
                        principalColumn: "ma_cong_viec");
                    table.ForeignKey(
                        name: "FK_cong_viec_xem_gan_day_ung_vien_FreelancerId",
                        column: x => x.FreelancerId,
                        principalTable: "ung_vien",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_cong_viec_xem_gan_day_JobId",
                table: "cong_viec_xem_gan_day",
                column: "JobId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cong_viec_xem_gan_day");
        }
    }
}
