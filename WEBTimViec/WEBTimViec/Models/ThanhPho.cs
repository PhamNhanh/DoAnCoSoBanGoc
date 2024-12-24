using System.ComponentModel.DataAnnotations;

namespace WEBTimViec.Models
{
    public class ThanhPho
    {
        [Key]
        public int ThanhPho_id { get; set; }
        public string? ThanhPho_name { get; set; }
        public string? kinhDo {  get; set; }
        public string? viDo { get; set; }
    }
}
