using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerPlatform.Infratructure.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataModelV31 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "chung_chi",
                table: "ung_vien",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "hoc_van",
                table: "ung_vien",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "kinh_nghiem",
                table: "ung_vien",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "thanh_tuu",
                table: "ung_vien",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "chung_chi",
                table: "ung_vien");

            migrationBuilder.DropColumn(
                name: "hoc_van",
                table: "ung_vien");

            migrationBuilder.DropColumn(
                name: "kinh_nghiem",
                table: "ung_vien");

            migrationBuilder.DropColumn(
                name: "thanh_tuu",
                table: "ung_vien");
        }
    }
}
