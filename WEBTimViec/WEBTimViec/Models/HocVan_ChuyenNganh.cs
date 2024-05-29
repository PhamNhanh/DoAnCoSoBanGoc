namespace WEBTimViec.Models
{
    public class HocVan_ChuyenNganh
    {
        public int Id { get; set; }

        public HocVan? hocVan { get; set; }
        public int HocVanId { get; set; }

        public ChuyenNganh? chuyenNganh { get; set; }
        public int? ChuyenNganhid { get; set; }
    }
}
