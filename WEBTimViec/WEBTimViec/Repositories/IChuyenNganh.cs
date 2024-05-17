using WEBTimViec.Models;

namespace WEBTimViec.Repositories
{
    public interface IChuyenNganh
    {
        Task<IEnumerable<ChuyenNganh>> GetAllAsync();
    }
}
