
using System.ComponentModel.DataAnnotations;

namespace WEBTimViec.Models
{
    public class NhomChuyenNganh
    {
        [Key]
        public int NhomChuyenNganhId { get; set; }
        public string? TenNhom { get; set; }

    }
}
