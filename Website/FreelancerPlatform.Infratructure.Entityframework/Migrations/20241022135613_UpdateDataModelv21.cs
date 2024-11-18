using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerPlatform.Infratructure.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataModelv21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "binh_luan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_binh_luan_bai_dang_FreelancerId",
                table: "binh_luan",
                column: "FreelancerId",
                principalTable: "bai_dang",
                principalColumn: "ma_bai_dang");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_binh_luan_bai_dang_FreelancerId",
                table: "binh_luan");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "binh_luan");
        }
    }
}
