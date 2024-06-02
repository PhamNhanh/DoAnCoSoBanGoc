using WEBTimViec.Models;

namespace WEBTimViec.Repositories
{
    public interface IUngTuyen
    {
        Task<IEnumerable<UngTuyen>> GetAllAsync();
        Task<UngTuyen> GetByIdAsync(int id);
        Task<IEnumerable<UngTuyen>> GetAllApplyByUserIdAsync(string id);
        Task<IEnumerable<UngTuyen>> GetAllApplyByCompanyIdAsync(string id);
        Task AddAsync(UngTuyen ungTuyen);
        Task UpdateAsync(UngTuyen ungTuyen);
        Task DeleteAsync(int id);
        Task<IEnumerable<UngTuyen>> GetUngTuyenByBaiTuyenDungIdAsync(int id);
        Task<IEnumerable<UngTuyen>> GetUngTuyenByUserIdAsync(string id);
        Task<int> CountUngTuyenAsync();
    }
}
