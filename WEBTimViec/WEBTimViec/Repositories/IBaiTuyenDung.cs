using WEBTimViec.Models;

namespace WEBTimViec.Repositories
{
    public interface IBaiTuyenDung
    {
        Task<IEnumerable<BaiTuyenDung>> GetAllAsync();
        Task<IEnumerable<BaiTuyenDung>> GetAllByCompanyIdAsync(string id);
        Task<BaiTuyenDung> GetByIdAsync(int id);
        Task AddAsync(BaiTuyenDung baiTuyenDung);
        Task UpdateAsync(BaiTuyenDung baiTuyenDung);
        Task DeleteAsync(int id);
        Task<IEnumerable<BaiTuyenDung>> GetBaiTuyenDungByUserIdAsync(string userId);
        Task<int> CountBaiTuyenDungAsync();
    }
}
