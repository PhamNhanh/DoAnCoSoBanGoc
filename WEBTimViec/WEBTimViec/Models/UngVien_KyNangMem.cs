namespace WEBTimViec.Models
{
    public class UngVien_KyNangMem
    {
        public int Id { get; set; }
        public int? UngVienid { get; set; }
        public ApplicationUser? applicationUser { get; set; }
        public KyNangMem? kyNangMem { get; set; }
        public int? KNMemid { get; set; }
    }
}
