using WEBTimViec.Models;

namespace WEBTimViec.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllAsync();
        Task<IEnumerable<ApplicationUser>> GetAllCompanyAsync();
        Task<IEnumerable<ApplicationUser>> GetAllUserAsync();
        Task<ApplicationUser> GetByIdAsync(string id);
        Task DeleteAsync(string id);
        Task<int> CountUsersInRoleNTDAsync();
        Task<int> CountUsersInRoleUVAsync();
        Task<int> CountUsersAsync();
        Task<int> CountNewNTDTodayAsync();
        Task<int> CountNewUVTodayAsync();
    }
}
