using System.ComponentModel.DataAnnotations;

namespace WEBTimViec.Models
{
    public class ChuyenNganh
    {
        [Key]
        public int ChuyenNganh_id { get; set; }
        public string? ChuyenNganh_name { get; set; }
        public List<HocVan_ChuyenNganh>? ungVien_ChuyenNganhs { get; set; }
        public List<BaiTuyenDung_ChuyenNganh>? baiTuyenDung_ChuyenNganhs { get; set; }
    }
}
