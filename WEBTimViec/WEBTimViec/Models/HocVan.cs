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
        public List<HocVan_ChuyenNganh>? hocVan_ChuyenNganhs { get; set; }

        [NotMapped]
        public List<int>? ChuyenNganhIds { get; set; } 
    }
}
