using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerPlatform.Infratructure.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataModelV18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FreelancerId",
                table: "ma_giao_dich",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ma_giao_dich_FreelancerId",
                table: "ma_giao_dich",
                column: "FreelancerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ma_giao_dich_ung_vien_FreelancerId",
                table: "ma_giao_dich",
                column: "FreelancerId",
                principalTable: "ung_vien",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ma_giao_dich_ung_vien_FreelancerId",
                table: "ma_giao_dich");

            migrationBuilder.DropIndex(
                name: "IX_ma_giao_dich_FreelancerId",
                table: "ma_giao_dich");

            migrationBuilder.DropColumn(
                name: "FreelancerId",
                table: "ma_giao_dich");
        }
    }
}
