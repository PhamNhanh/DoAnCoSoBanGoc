using WEBTimViec.Models;

namespace WEBTimViec.Repositories
{
    public interface IHocVan
    {
        Task<IEnumerable<HocVan>> GetAllAsync();
        Task<IEnumerable<HocVan>> GetByIdUserAsync(string id);
        Task<HocVan> GetByIdAsync(int id);
        Task AddAsync(HocVan hocVan);
        Task UpdateAsync(HocVan hocVan);
        Task DeleteAsync(int id);
    }
}
