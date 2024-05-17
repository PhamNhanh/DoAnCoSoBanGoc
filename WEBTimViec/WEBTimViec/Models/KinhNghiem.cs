using System.ComponentModel.DataAnnotations;

namespace WEBTimViec.Models
{
    public class KinhNghiem
    {
        [Key]
        public int KinhNghiem_id { get; set; }
        public string? NamKinhNghiem { get; set; }
    }
}
