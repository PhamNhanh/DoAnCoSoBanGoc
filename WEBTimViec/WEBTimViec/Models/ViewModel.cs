using System.ComponentModel.DataAnnotations.Schema;

namespace WEBTimViec.Models
{
    public class ViewModel
    {
        public IEnumerable<BaiTuyenDung> BaiTuyenDungs { get; set; }
        public BaiTuyenDung? baiTuyenDung { get; set; }
        [NotMapped]
        public int? BaiTuyenDungId { get; set; }
        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }
        public ApplicationUser? applicationUser { get; set; }
        [NotMapped]
        public int? apapplicationUserId { get; set; }
        public IEnumerable<ThanhPho> ThanhPhos { get; set; }
        public ThanhPho? thanhPho { get; set; }
        [NotMapped]
        public int? ThanhPhoId { get; set; }
        public IEnumerable<ChuyenNganh> ChuyenNganhs { get; set; }
        public ChuyenNganh? chuyenNganh { get; set; }
        [NotMapped]
        public int? chuyenNganhId { get; set; }
        public IEnumerable<KyNangMem> KyNangMems { get; set; }
        public KyNangMem? kyNangMem { get; set; }
        [NotMapped]
        public int? kyNangMemId { get; set; }
        public IEnumerable<HocVan> HocVans { get; set; }
        public HocVan? hocVan { get; set; }
        [NotMapped]
        public int? hocVanId { get; set; }
        public IEnumerable<TruongDaiHoc> TruongDaiHocs { get; set; }
        public TruongDaiHoc? truongDaiHoc { get; set; }
        [NotMapped]
        public int? truongDaiHocId { get; set; }

    }
}
