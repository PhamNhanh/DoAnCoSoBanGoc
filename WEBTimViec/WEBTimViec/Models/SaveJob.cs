namespace WEBTimViec.Models
{
    public class SavedJob
    {
        public int SavedJobId { get; set; }
        public string UserId { get; set; }
        public int BaiTuyenDungId { get; set; }
        public DateTime SavedDate { get; set; }

        // Mối quan hệ với bài tuyển dụng
        public BaiTuyenDung BaiTuyenDung { get; set; }

        // Mối quan hệ với người dùng (ứng viên)
        public ApplicationUser User { get; set; }

    }
}
