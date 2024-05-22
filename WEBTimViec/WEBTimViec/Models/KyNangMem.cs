using System.ComponentModel.DataAnnotations;

namespace WEBTimViec.Models
{
    public class KyNangMem
    {
        [Key]
        public int KyNangMem_id { get; set; }
        public string? KNMem_name { get; set; }
    }
}
