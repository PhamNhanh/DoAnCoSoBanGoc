namespace WEBTimViec.Models
{
    public class BaiTuyenDung_KyNangMem
    {
        public int Id { get; set; }
        public BaiTuyenDung? baiTuyenDung { get; set; }
        public int BaiTuyenDungid { get; set; }
        public KyNangMem? kyNangMem { get; set; }
        public int KyNangMemid { get; set; }
    }
}
