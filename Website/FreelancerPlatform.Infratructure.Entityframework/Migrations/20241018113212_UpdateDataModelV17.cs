using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerPlatform.Infratructure.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataModelV17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "noi_dung",
                table: "ma_giao_dich",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContractId",
                table: "ma_giao_dich",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "trang_thai_giao_dich",
                table: "ma_giao_dich",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ma_giao_dich_ContractId",
                table: "ma_giao_dich",
                column: "ContractId");

            migrationBuilder.AddForeignKey(
                name: "FK_ma_giao_dich_hop_dong_ContractId",
                table: "ma_giao_dich",
                column: "ContractId",
                principalTable: "hop_dong",
                principalColumn: "ma_hop_dong");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ma_giao_dich_hop_dong_ContractId",
                table: "ma_giao_dich");

            migrationBuilder.DropIndex(
                name: "IX_ma_giao_dich_ContractId",
                table: "ma_giao_dich");

            migrationBuilder.DropColumn(
                name: "ContractId",
                table: "ma_giao_dich");

            migrationBuilder.DropColumn(
                name: "trang_thai_giao_dich",
                table: "ma_giao_dich");

            migrationBuilder.AlterColumn<string>(
                name: "noi_dung",
                table: "ma_giao_dich",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
