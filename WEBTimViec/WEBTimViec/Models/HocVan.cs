using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEBTimViec.Models
{
    public class HocVan
    {
        [Key]
        public int HocVan_id { get; set; }
        public float GPA { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayTotNghiep { get; set; }
        public TruongDaiHoc? truongDaiHoc { get; set; }
        public int TruongDaiHocid { get; set; }
        public ApplicationUser? applicationUser { get; set; }
        public string? applicationUserId { get; set; }
 
        public ChuyenNganh? chuyenNganhs { get; set; } 
        public int? chuyenNganhId { get; set; }
    }
}
