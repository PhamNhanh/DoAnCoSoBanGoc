using WEBTimViec.Models;

namespace WEBTimViec.Repositories
{
    public interface IKinhNghiem
    {
        Task<IEnumerable<KinhNghiem>> GetAllAsync();
    }
}
