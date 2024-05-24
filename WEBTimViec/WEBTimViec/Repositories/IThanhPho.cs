using WEBTimViec.Models;

namespace WEBTimViec.Repositories
{
    public interface IThanhPho
    {
        Task<IEnumerable<ThanhPho>> GetAllAsync();
/*        Task<IEnumerable<ThanhPho>> GetByIdUserAsync(string id);*/
        Task<ThanhPho> GetByIdAsync(int id);
        Task AddAsync(ThanhPho thanhPho);
        Task UpdateAsync(ThanhPho thanhPho);
        Task DeleteAsync(int id);
        Task<string> HienThiTenTP(int? Id);
    }
}
