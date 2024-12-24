using System.ComponentModel.DataAnnotations;

namespace WEBTimViec.Models
{
    public class TruongDaiHoc
    {
        [Key]
        public int TruongDaiHoc_id { get; set; }
        public string? TruongDaiHoc_name { get; set; }

        public string? url_logo { get; set; }
    }
}
