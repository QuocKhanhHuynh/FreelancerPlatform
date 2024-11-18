using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerPlatform.Infratructure.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataModelV32 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "so_gio_lam_viec_ngay",
                table: "cong_viec",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "so_ngay_du_kien_hoan_thanh",
                table: "cong_viec",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "yeu_cau",
                table: "cong_viec",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "so_gio_lam_viec_ngay",
                table: "cong_viec");

            migrationBuilder.DropColumn(
                name: "so_ngay_du_kien_hoan_thanh",
                table: "cong_viec");

            migrationBuilder.DropColumn(
                name: "yeu_cau",
                table: "cong_viec");
        }
    }
}
