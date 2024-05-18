using System.ComponentModel.DataAnnotations;

namespace WEBTimViec.Models
{
    public class HinhAnhBTD
    {
        [Key]
        public int HinhAnhBTD_id { get; set; }
        public string? image_url { get; set; }
        public BaiTuyenDung? BaiTuyenDung { get; set; }
    }
}
