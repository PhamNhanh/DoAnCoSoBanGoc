using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WEBTimViec.Models;

namespace WEBTimViec.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<BaiTuyenDung> baiTuyenDungs { get; set; }
        public DbSet<BaiTuyenDung_ChuyenNganh> baiTuyenDung_ChuyenNganhs { get; set; }
        public DbSet<BaiTuyenDung_ViTri> baiTuyenDung_ViTris { get; set; }
        public DbSet<ChuyenNganh> chuyenNganhs { get; set; }
        public DbSet<HinhAnhBTD> hinhAnhBTDs { get; set; }
        public DbSet<HinhAnhNTD> hinhAnhNTDs { get; set; }
        public DbSet<HocVan> hocVans { get; set; }
        public DbSet<KinhNghiem> kinhNghiems { get; set; }
        public DbSet<KyNangMem> kyNangMems { get; set; }
/*        public DbSet<NhaTuyenDung> nhaTuyenDungs { get; set; }*/
        public DbSet<ThanhPho> thanhPhos { get; set; }
        public DbSet<TruongDaiHoc> truongDaiHocs { get; set; }
        public DbSet<UngTuyen> ungTuyens { get; set; }
/*        public DbSet<UngVien> ungViens { get; set; }*/
        public DbSet<UngVien_ChuyenNganh> ungVien_ChuyenNganhs { get; set; }
        public DbSet<UngVien_KyNangMem> ungVien_KyNangMems { get; set; }
        public DbSet<ViTriCongViec> viTriCongViecs { get; set; }

    }
}
