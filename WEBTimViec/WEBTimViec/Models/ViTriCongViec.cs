using System.ComponentModel.DataAnnotations;

namespace WEBTimViec.Models
{
    public class ViTriCongViec
    {
        [Key]
        public int ViTriCongViec_id { get; set; }
        public string? ViTriCongViec_name { get; set; }
        public List<BaiTuyenDung_ViTri>? baiTuyenDung_ViTris { get; set; }

    }
}
