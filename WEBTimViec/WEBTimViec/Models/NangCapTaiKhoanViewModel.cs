namespace WEBTimViec.Models
{
    public class NangCapTaiKhoanViewModel
    {
        // Danh sách các loại tài khoản
        public List<LoaiTaiKhoan>? LoaiTaiKhoanList { get; set; }

        // Loại tài khoản hiện tại của người dùng
        public int? CurrentLoaiTaiKhoanID { get; set; }

        // ID của loại tài khoản được chọn trong form
        public int? SelectedLoaiTaiKhoanID { get; set; }
    }
}
