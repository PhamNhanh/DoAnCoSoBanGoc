using System.Collections.Generic;
using System.Threading.Tasks;
using WEBTimViec.Models;

namespace WEBTimViec.Repositories
{
    public interface IHocVan
    {
        Task<IEnumerable<HocVan>> GetAllAsync();
        Task<HocVan> GetByIdAsync(int id);
        Task AddAsync(HocVan hocVan);
        Task UpdateAsync(HocVan hocVan);
        Task DeleteAsync(int id);
        Task<IEnumerable<HocVan>> GetByIdUserAsync(string id);
    }
}
