using System.ComponentModel.DataAnnotations;

namespace WEBTimViec.Models
{
    public class HinhAnhNTD
    {
        [Key]
        public int HinhAnhNTD_id { get; set; }
        public string? image_url { get; set; }
        public ApplicationUser? applicationUser {  get; set; }


    }
}
