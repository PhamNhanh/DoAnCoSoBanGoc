using WEBTimViec.Models;

namespace WEBTimViec.Repositories
{
    public interface IKyNangMem
    {
        Task AddAsync(KyNangMem kyNangMem);
        Task<IEnumerable<KyNangMem>> GetAllAsync();
    }
}
