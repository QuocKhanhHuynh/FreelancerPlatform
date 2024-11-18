using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreelancerPlatform.Infratructure.Entityframework.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dich_vu_cho-ung_vien",
                columns: table => new
                {
                    ma_dich_vu_cho_ung_vien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten_dich_vu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phi_dich_vu = table.Column<int>(type: "int", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dich_vu_cho-ung_vien", x => x.ma_dich_vu_cho_ung_vien);
                });

            migrationBuilder.CreateTable(
                name: "ky_nang",
                columns: table => new
                {
                    ma_ky_nang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten_ky_nang = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ky_nang", x => x.ma_ky_nang);
                });

            migrationBuilder.CreateTable(
                name: "linh_vuc_hoat_dong",
                columns: table => new
                {
                    linh_vuc_hoat_dong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten_linh_vuc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_linh_vuc_hoat_dong", x => x.linh_vuc_hoat_dong);
                });

            migrationBuilder.CreateTable(
                name: "ma_giao_dich",
                columns: table => new
                {
                    ma_giao_dich = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    so_tien = table.Column<int>(type: "int", nullable: false),
                    noi_dung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ma_giao_dich", x => x.ma_giao_dich);
                });

            migrationBuilder.CreateTable(
                name: "quan_tri_vien",
                columns: table => new
                {
                    ma_quan_tri_vien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten_tai_khoan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mat_khau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ho_ten = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    so_dien_thoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    trang_thai_hoat_dong = table.Column<bool>(type: "bit", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quan_tri_vien", x => x.ma_quan_tri_vien);
                });

            migrationBuilder.CreateTable(
                name: "quyen",
                columns: table => new
                {
                    ma_quyen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten_quyen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quyen", x => x.ma_quyen);
                });

            migrationBuilder.CreateTable(
                name: "vai_tro_quan_tri",
                columns: table => new
                {
                    ma_vai_tro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten_vai_tro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vai_tro_quan_tri", x => x.ma_vai_tro);
                });

            migrationBuilder.CreateTable(
                name: "ung_vien",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mat_khau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ho_ten_dem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ten = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ngay_sinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    gioi_tinh = table.Column<int>(type: "int", nullable: true),
                    so_dien_thoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    duong_dan_anh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    so_tai_khoan_ngan_hang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ten_tai_khoan_ngan_hang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gioi_thieu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    que_quan = table.Column<int>(type: "int", nullable: true),
                    xac_thuc_thong_tin_ca_nhan = table.Column<bool>(type: "bit", nullable: false),
                    PaymentVerification = table.Column<bool>(type: "bit", nullable: false),
                    do_uu_tien = table.Column<int>(type: "int", nullable: false),
                    SystemManagementId = table.Column<int>(type: "int", nullable: true),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ung_vien", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ung_vien_quan_tri_vien_SystemManagementId",
                        column: x => x.SystemManagementId,
                        principalTable: "quan_tri_vien",
                        principalColumn: "ma_quan_tri_vien");
                });

            migrationBuilder.CreateTable(
                name: "quyen_vai_tro",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quyen_vai_tro", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_quyen_vai_tro_quyen_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "quyen",
                        principalColumn: "ma_quyen");
                    table.ForeignKey(
                        name: "FK_quyen_vai_tro_vai_tro_quan_tri_RoleId",
                        column: x => x.RoleId,
                        principalTable: "vai_tro_quan_tri",
                        principalColumn: "ma_vai_tro");
                });

            migrationBuilder.CreateTable(
                name: "SystemManagementRole",
                columns: table => new
                {
                    SystemManagementId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemManagementRole", x => new { x.SystemManagementId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_SystemManagementRole_quan_tri_vien_SystemManagementId",
                        column: x => x.SystemManagementId,
                        principalTable: "quan_tri_vien",
                        principalColumn: "ma_quan_tri_vien");
                    table.ForeignKey(
                        name: "FK_SystemManagementRole_vai_tro_quan_tri_RoleId",
                        column: x => x.RoleId,
                        principalTable: "vai_tro_quan_tri",
                        principalColumn: "ma_vai_tro");
                });

            migrationBuilder.CreateTable(
                name: "bao_cao",
                columns: table => new
                {
                    ma_bao_cao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    noi_dung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    loai_bao_cao = table.Column<int>(type: "int", nullable: false),
                    FreelancerId = table.Column<int>(type: "int", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bao_cao", x => x.ma_bao_cao);
                    table.ForeignKey(
                        name: "FK_bao_cao_ung_vien_FreelancerId",
                        column: x => x.FreelancerId,
                        principalTable: "ung_vien",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "cong_viec",
                columns: table => new
                {
                    ma_cong_viec = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ten_viec = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mo_ta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    luong_dau = table.Column<int>(type: "int", nullable: false),
                    luong_cuoi = table.Column<int>(type: "int", nullable: false),
                    do_uu_tien = table.Column<int>(type: "int", nullable: false),
                    an_cong_viec = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    FreelancerId = table.Column<int>(type: "int", nullable: false),
                    SystemManagementId = table.Column<int>(type: "int", nullable: true),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cong_viec", x => x.ma_cong_viec);
                    table.ForeignKey(
                        name: "FK_cong_viec_linh_vuc_hoat_dong_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "linh_vuc_hoat_dong",
                        principalColumn: "linh_vuc_hoat_dong");
                    table.ForeignKey(
                        name: "FK_cong_viec_quan_tri_vien_SystemManagementId",
                        column: x => x.SystemManagementId,
                        principalTable: "quan_tri_vien",
                        principalColumn: "ma_quan_tri_vien");
                    table.ForeignKey(
                        name: "FK_cong_viec_ung_vien_FreelancerId",
                        column: x => x.FreelancerId,
                        principalTable: "ung_vien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dich_vu_ung_vien_yeu_cau",
                columns: table => new
                {
                    ma_ung_vien_yeu_cau = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    trang_thai = table.Column<int>(type: "int", nullable: false),
                    ServiceForFreelancerId = table.Column<int>(type: "int", nullable: false),
                    FreelancerId = table.Column<int>(type: "int", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dich_vu_ung_vien_yeu_cau", x => x.ma_ung_vien_yeu_cau);
                    table.ForeignKey(
                        name: "FK_dich_vu_ung_vien_yeu_cau_dich_vu_cho-ung_vien_ServiceForFreelancerId",
                        column: x => x.ServiceForFreelancerId,
                        principalTable: "dich_vu_cho-ung_vien",
                        principalColumn: "ma_dich_vu_cho_ung_vien");
                    table.ForeignKey(
                        name: "FK_dich_vu_ung_vien_yeu_cau_ung_vien_FreelancerId",
                        column: x => x.FreelancerId,
                        principalTable: "ung_vien",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FreelancerSkill",
                columns: table => new
                {
                    FreelancerId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreelancerSkill", x => new { x.FreelancerId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_FreelancerSkill_ky_nang_SkillId",
                        column: x => x.SkillId,
                        principalTable: "ky_nang",
                        principalColumn: "ma_ky_nang");
                    table.ForeignKey(
                        name: "FK_FreelancerSkill_ung_vien_FreelancerId",
                        column: x => x.FreelancerId,
                        principalTable: "ung_vien",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ung_vien_linh_vuc",
                columns: table => new
                {
                    FreelancerId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ung_vien_linh_vuc", x => new { x.FreelancerId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_ung_vien_linh_vuc_linh_vuc_hoat_dong_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "linh_vuc_hoat_dong",
                        principalColumn: "linh_vuc_hoat_dong");
                    table.ForeignKey(
                        name: "FK_ung_vien_linh_vuc_ung_vien_FreelancerId",
                        column: x => x.FreelancerId,
                        principalTable: "ung_vien",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ung_vien_tim_nang",
                columns: table => new
                {
                    ma_ung_vien_tim_nang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FreelancerId = table.Column<int>(type: "int", nullable: false),
                    FreelancerPotientId = table.Column<int>(type: "int", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ung_vien_tim_nang", x => x.ma_ung_vien_tim_nang);
                    table.ForeignKey(
                        name: "FK_ung_vien_tim_nang_ung_vien_FreelancerId",
                        column: x => x.FreelancerId,
                        principalTable: "ung_vien",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FavoriteJobs",
                columns: table => new
                {
                    JobId = table.Column<int>(type: "int", nullable: false),
                    FreelancerId = table.Column<int>(type: "int", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteJobs", x => new { x.FreelancerId, x.JobId });
                    table.ForeignKey(
                        name: "FK_FavoriteJobs_cong_viec_JobId",
                        column: x => x.JobId,
                        principalTable: "cong_viec",
                        principalColumn: "ma_cong_viec");
                    table.ForeignKey(
                        name: "FK_FavoriteJobs_ung_vien_FreelancerId",
                        column: x => x.FreelancerId,
                        principalTable: "ung_vien",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    ma_loi_moi = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ma_ung_vien = table.Column<int>(type: "int", nullable: false),
                    ma_nha_tuyen_dung = table.Column<int>(type: "int", nullable: false),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.ma_loi_moi);
                    table.ForeignKey(
                        name: "FK_Offers_cong_viec_JobId",
                        column: x => x.JobId,
                        principalTable: "cong_viec",
                        principalColumn: "ma_cong_viec");
                    table.ForeignKey(
                        name: "FK_Offers_ung_vien_ma_ung_vien",
                        column: x => x.ma_ung_vien,
                        principalTable: "ung_vien",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ung_tuyen_viec",
                columns: table => new
                {
                    ma_ung_tuyen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    luong_de_xuat = table.Column<int>(type: "int", nullable: false),
                    so_ngay_hoan_thanh = table.Column<int>(type: "int", nullable: false),
                    gioi_thieu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FreelancerId = table.Column<int>(type: "int", nullable: false),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    ngay_tao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ngay_cap_nhat = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ung_tuyen_viec", x => x.ma_ung_tuyen);
                    table.ForeignKey(
                        name: "FK_ung_tuyen_viec_cong_viec_JobId",
                        column: x => x.JobId,
                        principalTable: "cong_viec",
                        principalColumn: "ma_cong_viec");
                    table.ForeignKey(
                        name: "FK_ung_tuyen_viec_ung_vien_FreelancerId",
                        column: x => x.FreelancerId,
                        principalTable: "ung_vien",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_bao_cao_FreelancerId",
                table: "bao_cao",
                column: "FreelancerId");

            migrationBuilder.CreateIndex(
                name: "IX_cong_viec_CategoryId",
                table: "cong_viec",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_cong_viec_FreelancerId",
                table: "cong_viec",
                column: "FreelancerId");

            migrationBuilder.CreateIndex(
                name: "IX_cong_viec_SystemManagementId",
                table: "cong_viec",
                column: "SystemManagementId");

            migrationBuilder.CreateIndex(
                name: "IX_dich_vu_ung_vien_yeu_cau_FreelancerId",
                table: "dich_vu_ung_vien_yeu_cau",
                column: "FreelancerId");

            migrationBuilder.CreateIndex(
                name: "IX_dich_vu_ung_vien_yeu_cau_ServiceForFreelancerId",
                table: "dich_vu_ung_vien_yeu_cau",
                column: "ServiceForFreelancerId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteJobs_JobId",
                table: "FavoriteJobs",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_FreelancerSkill_SkillId",
                table: "FreelancerSkill",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_JobId",
                table: "Offers",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_ma_ung_vien",
                table: "Offers",
                column: "ma_ung_vien");

            migrationBuilder.CreateIndex(
                name: "IX_quyen_vai_tro_PermissionId",
                table: "quyen_vai_tro",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemManagementRole_RoleId",
                table: "SystemManagementRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ung_tuyen_viec_FreelancerId",
                table: "ung_tuyen_viec",
                column: "FreelancerId");

            migrationBuilder.CreateIndex(
                name: "IX_ung_tuyen_viec_JobId",
                table: "ung_tuyen_viec",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_ung_vien_SystemManagementId",
                table: "ung_vien",
                column: "SystemManagementId");

            migrationBuilder.CreateIndex(
                name: "IX_ung_vien_linh_vuc_CategoryId",
                table: "ung_vien_linh_vuc",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ung_vien_tim_nang_FreelancerId",
                table: "ung_vien_tim_nang",
                column: "FreelancerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bao_cao");

            migrationBuilder.DropTable(
                name: "dich_vu_ung_vien_yeu_cau");

            migrationBuilder.DropTable(
                name: "FavoriteJobs");

            migrationBuilder.DropTable(
                name: "FreelancerSkill");

            migrationBuilder.DropTable(
                name: "ma_giao_dich");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "quyen_vai_tro");

            migrationBuilder.DropTable(
                name: "SystemManagementRole");

            migrationBuilder.DropTable(
                name: "ung_tuyen_viec");

            migrationBuilder.DropTable(
                name: "ung_vien_linh_vuc");

            migrationBuilder.DropTable(
                name: "ung_vien_tim_nang");

            migrationBuilder.DropTable(
                name: "dich_vu_cho-ung_vien");

            migrationBuilder.DropTable(
                name: "ky_nang");

            migrationBuilder.DropTable(
                name: "quyen");

            migrationBuilder.DropTable(
                name: "vai_tro_quan_tri");

            migrationBuilder.DropTable(
                name: "cong_viec");

            migrationBuilder.DropTable(
                name: "linh_vuc_hoat_dong");

            migrationBuilder.DropTable(
                name: "ung_vien");

            migrationBuilder.DropTable(
                name: "quan_tri_vien");
        }
    }
}
