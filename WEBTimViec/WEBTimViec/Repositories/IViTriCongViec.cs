using WEBTimViec.Models;

namespace WEBTimViec.Repositories
{
    public interface IViTriCongViec
    {
        Task AddAsync(ViTriCongViec viTriCongViec);
        Task<IEnumerable<ViTriCongViec>> GetAllAsync();
    }
}
