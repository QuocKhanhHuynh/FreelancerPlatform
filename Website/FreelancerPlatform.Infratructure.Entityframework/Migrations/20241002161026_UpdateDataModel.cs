using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerPlatform.Infratructure.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "loai_cong_viec",
                table: "cong_viec",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "loai_luong",
                table: "cong_viec",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "JobSkill",
                columns: table => new
                {
                    JobId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSkill", x => new { x.JobId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_JobSkill_cong_viec_JobId",
                        column: x => x.JobId,
                        principalTable: "cong_viec",
                        principalColumn: "ma_cong_viec");
                    table.ForeignKey(
                        name: "FK_JobSkill_ky_nang_SkillId",
                        column: x => x.SkillId,
                        principalTable: "ky_nang",
                        principalColumn: "ma_ky_nang");
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobSkill_SkillId",
                table: "JobSkill",
                column: "SkillId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobSkill");

            migrationBuilder.DropColumn(
                name: "loai_cong_viec",
                table: "cong_viec");

            migrationBuilder.DropColumn(
                name: "loai_luong",
                table: "cong_viec");
        }
    }
}
