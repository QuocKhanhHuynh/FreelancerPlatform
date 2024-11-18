using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerPlatform.Infratructure.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataModelv22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_binh_luan_bai_dang_FreelancerId",
                table: "binh_luan");

            migrationBuilder.CreateIndex(
                name: "IX_binh_luan_PostId",
                table: "binh_luan",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_binh_luan_bai_dang_PostId",
                table: "binh_luan",
                column: "PostId",
                principalTable: "bai_dang",
                principalColumn: "ma_bai_dang");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_binh_luan_bai_dang_PostId",
                table: "binh_luan");

            migrationBuilder.DropIndex(
                name: "IX_binh_luan_PostId",
                table: "binh_luan");

            migrationBuilder.AddForeignKey(
                name: "FK_binh_luan_bai_dang_FreelancerId",
                table: "binh_luan",
                column: "FreelancerId",
                principalTable: "bai_dang",
                principalColumn: "ma_bai_dang");
        }
    }
}
