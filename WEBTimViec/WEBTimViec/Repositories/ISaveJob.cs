using WEBTimViec.Models;

namespace WEBTimViec.Repositories
{
    public interface ISaveJob
    {
        Task<IEnumerable<SavedJob>> GetSavedJobsAsync(string userId); // Thay đổi userId thành string
        Task SaveJobAsync(string userId, int baiTuyenDungId);
    }
}
