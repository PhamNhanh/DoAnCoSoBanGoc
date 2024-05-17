using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEBTimViec.Migrations
{
    /// <inheritdoc />
    public partial class identity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "chuyenNganhs",
                columns: table => new
                {
                    ChuyenNganh_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChuyenNganh_name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chuyenNganhs", x => x.ChuyenNganh_id);
                });

            migrationBuilder.CreateTable(
                name: "hinhAnhBTDs",
                columns: table => new
                {
                    HinhAnhBTD_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    image_url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hinhAnhBTDs", x => x.HinhAnhBTD_id);
                });

            migrationBuilder.CreateTable(
                name: "hinhAnhNTDs",
                columns: table => new
                {
                    HinhAnhNTD_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    image_url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hinhAnhNTDs", x => x.HinhAnhNTD_id);
                });

            migrationBuilder.CreateTable(
                name: "kinhNghiems",
                columns: table => new
                {
                    KinhNghiem_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NamKinhNghiem = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kinhNghiems", x => x.KinhNghiem_id);
                });

            migrationBuilder.CreateTable(
                name: "kyNangMems",
                columns: table => new
                {
                    KNMem_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KNMem_name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kyNangMems", x => x.KNMem_id);
                });

            migrationBuilder.CreateTable(
                name: "thanhPhos",
                columns: table => new
                {
                    ThanhPho_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThanhPho_name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_thanhPhos", x => x.ThanhPho_id);
                });

            migrationBuilder.CreateTable(
                name: "truongDaiHocs",
                columns: table => new
                {
                    TruongDaiHoc_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TruongDaiHoc_name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_truongDaiHocs", x => x.TruongDaiHoc_id);
                });

            migrationBuilder.CreateTable(
                name: "viTriCongViecs",
                columns: table => new
                {
                    ViTriCongViec_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ViTriCongViec_name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_viTriCongViecs", x => x.ViTriCongViec_id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "hocVans",
                columns: table => new
                {
                    HocVan_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GPA = table.Column<float>(type: "real", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayTotNghiep = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TruongDaiHocid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hocVans", x => x.HocVan_id);
                    table.ForeignKey(
                        name: "FK_hocVans_truongDaiHocs_TruongDaiHocid",
                        column: x => x.TruongDaiHocid,
                        principalTable: "truongDaiHocs",
                        principalColumn: "TruongDaiHoc_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    User_id = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image_url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SDT_UngVien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TuGioiThieu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HocVanid = table.Column<int>(type: "int", nullable: true),
                    NhaTuyenDung_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GioiThieuNhaTuyenDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SDTNhaTuyenDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThoiGianTao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThoiGianCapNhat = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HinhAnhNTDid = table.Column<int>(type: "int", nullable: false),
                    ThanhPhoid = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_hinhAnhNTDs_HinhAnhNTDid",
                        column: x => x.HinhAnhNTDid,
                        principalTable: "hinhAnhNTDs",
                        principalColumn: "HinhAnhNTD_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_hocVans_HocVanid",
                        column: x => x.HocVanid,
                        principalTable: "hocVans",
                        principalColumn: "HocVan_id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_thanhPhos_ThanhPhoid",
                        column: x => x.ThanhPhoid,
                        principalTable: "thanhPhos",
                        principalColumn: "ThanhPho_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "baiTuyenDungs",
                columns: table => new
                {
                    BaiTuyenDung_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenBaiTuyenDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MoTaCongViec = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YeuCauKyNang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhucLoi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Luong_min = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Luong_max = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    KieuCongViec = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThoiGianDangBai = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ThoiGianCapNhat = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HinhAnhBTDid = table.Column<int>(type: "int", nullable: true),
                    ThanhPhoid = table.Column<int>(type: "int", nullable: true),
                    KinhNghiemid = table.Column<int>(type: "int", nullable: false),
                    applicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_baiTuyenDungs", x => x.BaiTuyenDung_id);
                    table.ForeignKey(
                        name: "FK_baiTuyenDungs_AspNetUsers_applicationUserId",
                        column: x => x.applicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_baiTuyenDungs_hinhAnhBTDs_HinhAnhBTDid",
                        column: x => x.HinhAnhBTDid,
                        principalTable: "hinhAnhBTDs",
                        principalColumn: "HinhAnhBTD_id");
                    table.ForeignKey(
                        name: "FK_baiTuyenDungs_kinhNghiems_KinhNghiemid",
                        column: x => x.KinhNghiemid,
                        principalTable: "kinhNghiems",
                        principalColumn: "KinhNghiem_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_baiTuyenDungs_thanhPhos_ThanhPhoid",
                        column: x => x.ThanhPhoid,
                        principalTable: "thanhPhos",
                        principalColumn: "ThanhPho_id");
                });

            migrationBuilder.CreateTable(
                name: "ungVien_ChuyenNganhs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UngVienid = table.Column<int>(type: "int", nullable: true),
                    applicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ChuyenNganhid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ungVien_ChuyenNganhs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ungVien_ChuyenNganhs_AspNetUsers_applicationUserId",
                        column: x => x.applicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ungVien_ChuyenNganhs_chuyenNganhs_ChuyenNganhid",
                        column: x => x.ChuyenNganhid,
                        principalTable: "chuyenNganhs",
                        principalColumn: "ChuyenNganh_id");
                });

            migrationBuilder.CreateTable(
                name: "ungVien_KyNangMems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UngVienid = table.Column<int>(type: "int", nullable: true),
                    applicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    kyNangMemKNMem_id = table.Column<int>(type: "int", nullable: true),
                    KNMemid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ungVien_KyNangMems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ungVien_KyNangMems_AspNetUsers_applicationUserId",
                        column: x => x.applicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ungVien_KyNangMems_kyNangMems_kyNangMemKNMem_id",
                        column: x => x.kyNangMemKNMem_id,
                        principalTable: "kyNangMems",
                        principalColumn: "KNMem_id");
                });

            migrationBuilder.CreateTable(
                name: "baiTuyenDung_ChuyenNganhs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaiTuyenDungid = table.Column<int>(type: "int", nullable: false),
                    ChuyenNganhid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_baiTuyenDung_ChuyenNganhs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_baiTuyenDung_ChuyenNganhs_baiTuyenDungs_BaiTuyenDungid",
                        column: x => x.BaiTuyenDungid,
                        principalTable: "baiTuyenDungs",
                        principalColumn: "BaiTuyenDung_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_baiTuyenDung_ChuyenNganhs_chuyenNganhs_ChuyenNganhid",
                        column: x => x.ChuyenNganhid,
                        principalTable: "chuyenNganhs",
                        principalColumn: "ChuyenNganh_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "baiTuyenDung_ViTris",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaiTuyenDungid = table.Column<int>(type: "int", nullable: false),
                    ViTriCongViecid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_baiTuyenDung_ViTris", x => x.Id);
                    table.ForeignKey(
                        name: "FK_baiTuyenDung_ViTris_baiTuyenDungs_BaiTuyenDungid",
                        column: x => x.BaiTuyenDungid,
                        principalTable: "baiTuyenDungs",
                        principalColumn: "BaiTuyenDung_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_baiTuyenDung_ViTris_viTriCongViecs_ViTriCongViecid",
                        column: x => x.ViTriCongViecid,
                        principalTable: "viTriCongViecs",
                        principalColumn: "ViTriCongViec_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ungTuyens",
                columns: table => new
                {
                    UngTuyen_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    url_CV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThuGioiThieu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThoiGianUngTuyen = table.Column<DateTime>(type: "datetime2", nullable: true),
                    applicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    application_Userid = table.Column<int>(type: "int", nullable: true),
                    BaiTuyenDungid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ungTuyens", x => x.UngTuyen_id);
                    table.ForeignKey(
                        name: "FK_ungTuyens_AspNetUsers_applicationUserId",
                        column: x => x.applicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ungTuyens_baiTuyenDungs_BaiTuyenDungid",
                        column: x => x.BaiTuyenDungid,
                        principalTable: "baiTuyenDungs",
                        principalColumn: "BaiTuyenDung_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_HinhAnhNTDid",
                table: "AspNetUsers",
                column: "HinhAnhNTDid");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_HocVanid",
                table: "AspNetUsers",
                column: "HocVanid",
                unique: true,
                filter: "[HocVanid] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ThanhPhoid",
                table: "AspNetUsers",
                column: "ThanhPhoid");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_baiTuyenDung_ChuyenNganhs_BaiTuyenDungid",
                table: "baiTuyenDung_ChuyenNganhs",
                column: "BaiTuyenDungid");

            migrationBuilder.CreateIndex(
                name: "IX_baiTuyenDung_ChuyenNganhs_ChuyenNganhid",
                table: "baiTuyenDung_ChuyenNganhs",
                column: "ChuyenNganhid");

            migrationBuilder.CreateIndex(
                name: "IX_baiTuyenDung_ViTris_BaiTuyenDungid",
                table: "baiTuyenDung_ViTris",
                column: "BaiTuyenDungid");

            migrationBuilder.CreateIndex(
                name: "IX_baiTuyenDung_ViTris_ViTriCongViecid",
                table: "baiTuyenDung_ViTris",
                column: "ViTriCongViecid");

            migrationBuilder.CreateIndex(
                name: "IX_baiTuyenDungs_applicationUserId",
                table: "baiTuyenDungs",
                column: "applicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_baiTuyenDungs_HinhAnhBTDid",
                table: "baiTuyenDungs",
                column: "HinhAnhBTDid");

            migrationBuilder.CreateIndex(
                name: "IX_baiTuyenDungs_KinhNghiemid",
                table: "baiTuyenDungs",
                column: "KinhNghiemid");

            migrationBuilder.CreateIndex(
                name: "IX_baiTuyenDungs_ThanhPhoid",
                table: "baiTuyenDungs",
                column: "ThanhPhoid");

            migrationBuilder.CreateIndex(
                name: "IX_hocVans_TruongDaiHocid",
                table: "hocVans",
                column: "TruongDaiHocid");

            migrationBuilder.CreateIndex(
                name: "IX_ungTuyens_applicationUserId",
                table: "ungTuyens",
                column: "applicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ungTuyens_BaiTuyenDungid",
                table: "ungTuyens",
                column: "BaiTuyenDungid");

            migrationBuilder.CreateIndex(
                name: "IX_ungVien_ChuyenNganhs_applicationUserId",
                table: "ungVien_ChuyenNganhs",
                column: "applicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ungVien_ChuyenNganhs_ChuyenNganhid",
                table: "ungVien_ChuyenNganhs",
                column: "ChuyenNganhid");

            migrationBuilder.CreateIndex(
                name: "IX_ungVien_KyNangMems_applicationUserId",
                table: "ungVien_KyNangMems",
                column: "applicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ungVien_KyNangMems_kyNangMemKNMem_id",
                table: "ungVien_KyNangMems",
                column: "kyNangMemKNMem_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "baiTuyenDung_ChuyenNganhs");

            migrationBuilder.DropTable(
                name: "baiTuyenDung_ViTris");

            migrationBuilder.DropTable(
                name: "ungTuyens");

            migrationBuilder.DropTable(
                name: "ungVien_ChuyenNganhs");

            migrationBuilder.DropTable(
                name: "ungVien_KyNangMems");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "viTriCongViecs");

            migrationBuilder.DropTable(
                name: "baiTuyenDungs");

            migrationBuilder.DropTable(
                name: "chuyenNganhs");

            migrationBuilder.DropTable(
                name: "kyNangMems");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "hinhAnhBTDs");

            migrationBuilder.DropTable(
                name: "kinhNghiems");

            migrationBuilder.DropTable(
                name: "hinhAnhNTDs");

            migrationBuilder.DropTable(
                name: "hocVans");

            migrationBuilder.DropTable(
                name: "thanhPhos");

            migrationBuilder.DropTable(
                name: "truongDaiHocs");
        }
    }
}
