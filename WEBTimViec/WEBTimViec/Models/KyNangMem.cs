using System.ComponentModel.DataAnnotations;

namespace WEBTimViec.Models
{
    public class KyNangMem
    {
        [Key]
        public int KNMem_id { get; set; }
        public string? KNMem_name { get; set; }
        public List<UngVien_KyNangMem>? ungVien_KyNangMem { get; set; }
    }
}
