using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEBTimViec.Models;

namespace WEBTimViec.Repositories
{
    public class EFUserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public EFUserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("User");
            return usersInRole.ToList();
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }
        public async Task<IEnumerable<ApplicationUser>> GetAllCompanyAsync()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("Nhà Tuyển Dụng");
            return usersInRole.ToList();
        }
        public async Task<IEnumerable<ApplicationUser>> GetAllUserAsync()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("Ứng Viên");
            return usersInRole.ToList();
        }
        public async Task<int> CountUsersInRoleNTDAsync()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("Nhà Tuyển Dụng");
            return usersInRole.Count;
        }
        public async Task<int> CountUsersInRoleUVAsync()
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("Ứng Viên");
            return usersInRole.Count;
        }
        public async Task<int> CountUsersAsync()
        {
            var users = _userManager.Users; 
            return await users.CountAsync(); 
        }

        public async Task<int> CountNewUVTodayAsync()
        {
            DateTime today = DateTime.Today;

            var usersInRole = await _userManager.GetUsersInRoleAsync("Ứng Viên");

            if (usersInRole != null && usersInRole.Any())
            {
                var newUsersToday = await _userManager.Users
                    .Where(u => u.ThoiGianTao.HasValue && u.ThoiGianTao.Value.Date == today)
                    .ToListAsync();

                return newUsersToday.Count;
            }
            else
            {
                return 0;
            }
        }
        public async Task<int> CountNewNTDTodayAsync()
        {
            DateTime today = DateTime.Today;

            var usersInRole = await _userManager.GetUsersInRoleAsync("Nhà Tuyển Dụng");

            if (usersInRole != null && usersInRole.Any())
            {
                var newUsersToday = await _userManager.Users
                    .Where(u => u.ThoiGianTao.HasValue && u.ThoiGianTao.Value.Date == today)
                    .ToListAsync();

                return newUsersToday.Count;
            }
            else
            {
                return 0;
            }
        }

    }
}
