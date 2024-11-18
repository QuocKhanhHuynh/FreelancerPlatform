using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerPlatform.Infratructure.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataModelV26 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cong_viec_quan_tri_vien_SystemManagementId",
                table: "cong_viec");

            migrationBuilder.DropForeignKey(
                name: "FK_SystemManagementRole_quan_tri_vien_SystemManagementId",
                table: "SystemManagementRole");

            migrationBuilder.DropForeignKey(
                name: "FK_ung_vien_quan_tri_vien_SystemManagementId",
                table: "ung_vien");

            migrationBuilder.DropIndex(
                name: "IX_ung_vien_SystemManagementId",
                table: "ung_vien");

            migrationBuilder.DropIndex(
                name: "IX_cong_viec_SystemManagementId",
                table: "cong_viec");

            migrationBuilder.DropColumn(
                name: "SystemManagementId",
                table: "ung_vien");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "quan_tri_vien");

            migrationBuilder.DropColumn(
                name: "ho_ten",
                table: "quan_tri_vien");

            migrationBuilder.DropColumn(
                name: "trang_thai_hoat_dong",
                table: "quan_tri_vien");

            migrationBuilder.DropColumn(
                name: "SystemManagementId",
                table: "cong_viec");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemManagementRole_quan_tri_vien_SystemManagementId",
                table: "SystemManagementRole",
                column: "SystemManagementId",
                principalTable: "quan_tri_vien",
                principalColumn: "ma_quan_tri_vien",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SystemManagementRole_quan_tri_vien_SystemManagementId",
                table: "SystemManagementRole");

            migrationBuilder.AddColumn<int>(
                name: "SystemManagementId",
                table: "ung_vien",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "quan_tri_vien",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ho_ten",
                table: "quan_tri_vien",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "trang_thai_hoat_dong",
                table: "quan_tri_vien",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SystemManagementId",
                table: "cong_viec",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ung_vien_SystemManagementId",
                table: "ung_vien",
                column: "SystemManagementId");

            migrationBuilder.CreateIndex(
                name: "IX_cong_viec_SystemManagementId",
                table: "cong_viec",
                column: "SystemManagementId");

            migrationBuilder.AddForeignKey(
                name: "FK_cong_viec_quan_tri_vien_SystemManagementId",
                table: "cong_viec",
                column: "SystemManagementId",
                principalTable: "quan_tri_vien",
                principalColumn: "ma_quan_tri_vien");

            migrationBuilder.AddForeignKey(
                name: "FK_SystemManagementRole_quan_tri_vien_SystemManagementId",
                table: "SystemManagementRole",
                column: "SystemManagementId",
                principalTable: "quan_tri_vien",
                principalColumn: "ma_quan_tri_vien");

            migrationBuilder.AddForeignKey(
                name: "FK_ung_vien_quan_tri_vien_SystemManagementId",
                table: "ung_vien",
                column: "SystemManagementId",
                principalTable: "quan_tri_vien",
                principalColumn: "ma_quan_tri_vien");
        }
    }
}
