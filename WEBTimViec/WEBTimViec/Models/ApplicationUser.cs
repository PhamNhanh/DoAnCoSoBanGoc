using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBTimViec.Models
{
    public class ApplicationUser : IdentityUser
    {
        //Su dung chung
/*        [Key]
        public int User_id { get; set; }*/
        //Ung vien
   
     /*   public int UngVien_id { get; set; }*/
        public string? FullName { get; set; }
        //trong bang aspnet user co san cot email

       /* public string? email { get; set; }*/
        public string? image_url { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? SDT_UngVien { get; set; }
        public string? TuGioiThieu { get; set; }
        public HocVan? hocVan { get; set; }
        public int? HocVanid { get; set; }

        public List<UngVien_ChuyenNganh>? ungVien_ChuyenNganhs { get; set; }

        public string? NhaTuyenDung_name { get; set; }
        public string? GioiThieuNhaTuyenDung { get; set; }
        public string? DiaChi { get; set; }
        public string? SDTNhaTuyenDung { get; set; }
        public string? Website { get; set; }
        public DateTime? ThoiGianTao { get; set; }
        public DateTime? ThoiGianCapNhat { get; set; }
        public List<BaiTuyenDung> BaiTuyenDungs { get; set; }

    }
}
