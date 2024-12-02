namespace WEBTimViec.Models
{
    public class BaiTuyenDung_ViTri
    {
        public int Id { get; set; }
        public BaiTuyenDung? baiTuyenDung { get; set; }
        public int BaiTuyenDungid { get; set; }
        public ViTriCongViec? viTriCongViec { get; set; }
        public int ViTriCongViecid { get; set; }
    }
}
/**/