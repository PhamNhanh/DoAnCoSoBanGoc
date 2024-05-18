using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WEBTimViec.Models
{
    public class BaiTuyenDung
    {
        [Key]
        public int BaiTuyenDung_id { get; set; }
        public string? TenBaiTuyenDung { get; set; }
        public string? MoTaCongViec { get; set; }
        public string? YeuCauKyNang { get; set; }
        public string? PhucLoi { get; set; }
        public decimal? Luong_min { get; set; }
        public decimal? Luong_max { get; set; }
        public string? KieuCongViec { get; set; }
        public DateTime? ThoiGianDangBai { get; set; }
        public DateTime? ThoiGianCapNhat { get; set; }
        public List<HinhAnhBTD>? hinhAnhBTD { get; set; }
        public ThanhPho? thanhPho { get; set; }
        public int? thanhPhoid { get; set; }
        public KinhNghiem? kinhNghiem { get; set; }
        public int? kinhNghiemid { get; set; }
        public List<BaiTuyenDung_ViTri>? baiTuyenDung_ViTris { get; set; }
        public List<BaiTuyenDung_ChuyenNganh>? baiTuyenDung_ChuyenNganhs { get; set; }

        public ChuyenNganh? chuyenNganh { get; set; }
        public int? chuyenNganhid { get; set; }

/*        public int? applicationUserid { get; set; }*/
        public ApplicationUser? applicationUser { get; set; }

    }
}
