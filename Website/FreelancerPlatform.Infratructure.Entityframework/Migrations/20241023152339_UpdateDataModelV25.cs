using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerPlatform.Infratructure.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataModelV25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LikePosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    FreelancerId = table.Column<int>(type: "int", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikePosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LikePosts_bai_dang_PostId",
                        column: x => x.PostId,
                        principalTable: "bai_dang",
                        principalColumn: "ma_bai_dang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LikePosts_ung_vien_FreelancerId",
                        column: x => x.FreelancerId,
                        principalTable: "ung_vien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LikePosts_FreelancerId",
                table: "LikePosts",
                column: "FreelancerId");

            migrationBuilder.CreateIndex(
                name: "IX_LikePosts_PostId",
                table: "LikePosts",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LikePosts");
        }
    }
}
