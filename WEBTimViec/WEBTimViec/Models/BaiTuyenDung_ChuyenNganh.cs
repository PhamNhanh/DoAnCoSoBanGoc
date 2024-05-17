namespace WEBTimViec.Models
{
    public class BaiTuyenDung_ChuyenNganh
    {
        public int Id { get; set; }
        public BaiTuyenDung? baiTuyenDung {  get; set; }
        public int BaiTuyenDungid { get; set; }
        public ChuyenNganh? chuyenNganh { get; set; }
        public int ChuyenNganhid { get; set; }
    }
}
