using System.ComponentModel.DataAnnotations.Schema;
using PagedList;
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
        public int? applicationUserId { get; set; }
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
        [NotMapped]
        public string JobName { get; set; }
        public List<MajorViewModel> Majors { get; set; }
        public List<BaiTuyenDung>? BaiTuyenDung { get; set; }
        public X.PagedList.IPagedList<BaiTuyenDung> baiTuyenDungs { get; set; }
        public int TotalCount { get; set; }  // Tổng số bài tuyển dụng
        public int PageCount { get; set; }   // Tổng số trang
    }
}
