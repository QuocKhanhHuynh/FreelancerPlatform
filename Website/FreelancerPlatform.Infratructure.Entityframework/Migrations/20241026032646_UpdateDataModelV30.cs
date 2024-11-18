using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerPlatform.Infratructure.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataModelV30 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LikeComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    FreelancerId = table.Column<int>(type: "int", nullable: false),
                    CommentId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LikeComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LikeComments_binh_luan_CommentId",
                        column: x => x.CommentId,
                        principalTable: "binh_luan",
                        principalColumn: "ma_binh_luan");
                    table.ForeignKey(
                        name: "FK_LikeComments_binh_luan_CommentId1",
                        column: x => x.CommentId1,
                        principalTable: "binh_luan",
                        principalColumn: "ma_binh_luan");
                    table.ForeignKey(
                        name: "FK_LikeComments_ung_vien_FreelancerId",
                        column: x => x.FreelancerId,
                        principalTable: "ung_vien",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LikeComments_CommentId",
                table: "LikeComments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_LikeComments_CommentId1",
                table: "LikeComments",
                column: "CommentId1");

            migrationBuilder.CreateIndex(
                name: "IX_LikeComments_FreelancerId",
                table: "LikeComments",
                column: "FreelancerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LikeComments");
        }
    }
}
