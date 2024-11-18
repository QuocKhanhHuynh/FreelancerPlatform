using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerPlatform.Infratructure.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataModelV13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "danh_gia_doi_tac",
                table: "hop_dong",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "diem_doi_tac",
                table: "hop_dong",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "danh_gia_doi_tac",
                table: "hop_dong");

            migrationBuilder.DropColumn(
                name: "diem_doi_tac",
                table: "hop_dong");
        }
    }
}
