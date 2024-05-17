using WEBTimViec.Models;
using static WEBTimViec.Repositories.ITruongDaiHoc;

namespace WEBTimViec.Repositories
{
        public interface ITruongDaiHoc
        {
            Task<IEnumerable<TruongDaiHoc>> GetAllAsync();
        }
}
