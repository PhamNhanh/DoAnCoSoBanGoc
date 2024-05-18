using WEBTimViec.Models;

namespace WEBTimViec.Repositories
{
    public interface IChuyenNganh
    {
        Task AddAsync(ChuyenNganh chuyenNganh);
        Task<IEnumerable<ChuyenNganh>> GetAllAsync();
    }
}
