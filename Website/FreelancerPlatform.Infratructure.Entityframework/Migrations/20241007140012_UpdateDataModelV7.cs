using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerPlatform.Infratructure.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDataModelV7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "chat_tap_trung",
                columns: table => new
                {
                    ma_chat_tap_trung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_tap_trung", x => x.ma_chat_tap_trung);
                });

            migrationBuilder.CreateTable(
                name: "tin_nhan",
                columns: table => new
                {
                    ma_tin_nhan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    noi_dung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FreelancerId = table.Column<int>(type: "int", nullable: false),
                    HubChatId = table.Column<int>(type: "int", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tin_nhan", x => x.ma_tin_nhan);
                    table.ForeignKey(
                        name: "FK_tin_nhan_chat_tap_trung_HubChatId",
                        column: x => x.HubChatId,
                        principalTable: "chat_tap_trung",
                        principalColumn: "ma_chat_tap_trung");
                    table.ForeignKey(
                        name: "FK_tin_nhan_ung_vien_FreelancerId",
                        column: x => x.FreelancerId,
                        principalTable: "ung_vien",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tin_nhan_FreelancerId",
                table: "tin_nhan",
                column: "FreelancerId");

            migrationBuilder.CreateIndex(
                name: "IX_tin_nhan_HubChatId",
                table: "tin_nhan",
                column: "HubChatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tin_nhan");

            migrationBuilder.DropTable(
                name: "chat_tap_trung");
        }
    }
}
