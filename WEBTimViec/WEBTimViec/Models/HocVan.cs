using System.ComponentModel.DataAnnotations;

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
    }
}
