namespace WEBTimViec.Models
{
    public class UngVien_ChuyenNganh
    {
        public int Id { get; set; }

        public ApplicationUser? applicationUser { get; set; }


        public ChuyenNganh? chuyenNganh { get; set; }
        public int? ChuyenNganhid { get; set; }
    }
}
