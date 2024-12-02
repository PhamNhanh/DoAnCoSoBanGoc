using System.ComponentModel.DataAnnotations;

namespace WEBTimViec.Models
{
    public class LoaiTaiKhoan
    {
        [Key]
        public int loaiTaiKhoanId { get; set; }
        public string? tenLoaiTaiKhoan {  get; set; }
        public int soNgayDung { get; set; }
        public decimal gia {  get; set; }
        public int soBaiTuyenDung { get; set; }
    }
}
