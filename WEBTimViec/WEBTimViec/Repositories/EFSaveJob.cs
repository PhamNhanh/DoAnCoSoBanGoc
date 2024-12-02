using WEBTimViec.Data;
using WEBTimViec.Models;
using Microsoft.EntityFrameworkCore;

namespace WEBTimViec.Repositories
{
    public class EFSaveJob : ISaveJob
    {
        private readonly ApplicationDbContext _context;

        public EFSaveJob(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lưu công việc cho người dùng
        public async Task SaveJobAsync(string userId, int baiTuyenDungId)
        {
            var savedJob = new SavedJob
            {
                UserId = userId,
                BaiTuyenDungId = baiTuyenDungId,
                SavedDate = DateTime.Now
            };

            await _context.saveJobs.AddAsync(savedJob); // Thêm công việc đã lưu vào cơ sở dữ liệu
            await _context.SaveChangesAsync(); // Lưu thay đổi
        }

        // Lấy danh sách các công việc đã lưu cho người dùng
        public async Task<IEnumerable<SavedJob>> GetSavedJobsAsync(string userId)
        {
            return await _context.saveJobs
                                 .Where(sj => sj.UserId == userId)
                                 .Include(sj => sj.BaiTuyenDung)
                                 .ToListAsync(); // Trả về danh sách công việc đã lưu của người dùng
        }

        // Xóa công việc đã lưu
        public async Task RemoveSavedJobAsync(string userId, int baiTuyenDungId)
        {
            var savedJob = await _context.saveJobs
                                          .FirstOrDefaultAsync(sj => sj.UserId == userId && sj.BaiTuyenDungId == baiTuyenDungId);
            if (savedJob != null)
            {
                _context.saveJobs.Remove(savedJob);
                await _context.SaveChangesAsync();
            }
        }
    }
}
