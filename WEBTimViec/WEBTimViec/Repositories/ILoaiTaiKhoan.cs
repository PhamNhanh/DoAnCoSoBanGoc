using WEBTimViec.Models;

namespace WEBTimViec.Repositories
{
    public interface ILoaiTaiKhoan
    {
        Task<List<LoaiTaiKhoan>> GetAllLoaiTaiKhoanAsync();
        Task<LoaiTaiKhoan> GetLoaiTaiKhoanByIdAsync(int id);
        Task AddLoaiTaiKhoanAsync(LoaiTaiKhoan loaiTaiKhoan);
        Task UpdateLoaiTaiKhoanAsync(LoaiTaiKhoan loaiTaiKhoan);
        Task DeleteLoaiTaiKhoanAsync(int id);
        Task<string> GetTenLoaiTaiKhoanByUserIdAsync(string userId);
    }
}
