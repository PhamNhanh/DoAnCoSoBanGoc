using Microsoft.EntityFrameworkCore;
using WEBTimViec.Data;
using WEBTimViec.Models;

namespace WEBTimViec.Repositories
{
    public class EFUngTuyenRepository : IUngTuyen
    {
        private readonly ApplicationDbContext _context;
        public EFUngTuyenRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<UngTuyen>> GetAllAsync()
        {
            return await _context.ungTuyens
                .ToListAsync();
        }

        public async Task<IEnumerable<UngTuyen>> GetAllApplyByCompanyIdAsync(string id)
        {
            return await _context.ungTuyens
                .Where(b => b.BaiTuyenDung.applicationUser.Id == id)
                .ToListAsync();
        }


        public async Task<UngTuyen?> GetByIdAsync(int id)
        {
            return await _context.ungTuyens.FindAsync(id);
        }

        public async Task AddAsync(UngTuyen ungTuyen)
        {
            _context.ungTuyens.Add(ungTuyen);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UngTuyen ungTuyen)
        {
            _context.ungTuyens.Update(ungTuyen);
            await _context.SaveChangesAsync();
        }

        public async Task? DeleteAsync(int id)
        {
            var ungTuyen = await _context.ungTuyens.FindAsync(id);
            _context.ungTuyens.Remove(ungTuyen);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<UngTuyen>> GetAllApplyByUserIdAsync(string id)
        {
            //bao gồm danh mục, nếu không có sẽ ko ra danh mục
            var applicationDbContext = await _context.ungTuyens
                .Include(b => b.BaiTuyenDung)
                .Include(b => b.applicationUser)
                .Where(b => b.applicationUser.Id == id)
                .ToListAsync();

            return applicationDbContext;
        }
        public async Task<IEnumerable<UngTuyen>> GetApplicationsByApplicantIdAsync(string Id)
        {
            var applications = await _context.ungTuyens
                .Include(ut => ut.BaiTuyenDung)
                .ThenInclude(bt => bt.applicationUser) // Giả sử có thực thể Company liên quan đến BaiTuyenDung
                .Include(ut => ut.UngTuyen_id)
                .Where(ut => ut.applicationUser.Id == Id)
                .ToListAsync();

            return applications;
        }
        public async Task<IEnumerable<UngTuyen>> GetUngTuyenByBaiTuyenDungIdAsync(int id)
        {
            return await _context.ungTuyens
                .Where(u => u.BaiTuyenDungid == id)
                .ToListAsync();
        }
        public async Task<IEnumerable<UngTuyen>> GetUngTuyenByUserIdAsync(string id)
        {
            return await _context.ungTuyens
                .Where(u => u.applicationUser.Id == id)
                .ToListAsync();
        }
        public async Task<int> CountUngTuyenAsync()
        {
            return await _context.ungTuyens.CountAsync();
        }
        public async Task<int> CountUngTuyenTodayAsync()
        {
            var today = DateTime.Today;
            var count = await _context.ungTuyens
                .Where(ut => ut.ThoiGianUngTuyen.Value == today)
                .CountAsync();

            return count;
        }

    }
}
