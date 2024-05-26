using System.Collections.Generic;
using System.Threading.Tasks;
using WEBTimViec.Models;

namespace WEBTimViec.Repositories
{
    public interface IBaiTuyenDung_ChuyenNganh
    {
        Task<IEnumerable<ChuyenNganh>> GetAllAsync();
    }
}
