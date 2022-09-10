using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebTools.Migrations
{
    public partial class IntialDBSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportLists",
                columns: table => new
                {
                    IdBieuMau = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IDPhienBan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhienBan = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TenBM = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MaBM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NgayBanHanh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileLink = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    KhoaPhong = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    QuyTrinh = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ViTriIn = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CachIn = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    URD = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TheLoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhanMem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThaiSD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThaiPM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    STT = table.Column<int>(type: "int", nullable: true),
                    PhanMem1 = table.Column<int>(type: "int", nullable: false),
                    PhienBan1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportLists", x => x.IdBieuMau);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportLists");
        }
    }
}
