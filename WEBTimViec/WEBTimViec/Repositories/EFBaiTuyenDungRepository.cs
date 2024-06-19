using Microsoft.EntityFrameworkCore;
using WEBTimViec.Data;
using WEBTimViec.Models;

namespace WEBTimViec.Repositories
{
    public class EFBaiTuyenDungRepository : IBaiTuyenDung
    {
        private readonly ApplicationDbContext _context;
        public EFBaiTuyenDungRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BaiTuyenDung>> GetAllAsync()
        {
            //bao gồm danh mục, nếu không có sẽ ko ra danh mục
            var applicationDbContext = await _context.baiTuyenDungs
    .Include(b => b.thanhPho)
    .Include(b => b.kinhNghiem)
    .Include(b => b.chuyenNganh)
    .Include(b => b.applicationUser)
    .ToListAsync();

            return await _context.baiTuyenDungs.ToListAsync();
        }

        public async Task<IEnumerable<BaiTuyenDung>> GetAllByCompanyIdAsync(string id)
        {
            //bao gồm danh mục, nếu không có sẽ ko ra danh mục
            var applicationDbContext = await _context.baiTuyenDungs
    .Include(b => b.thanhPho)
    .Include(b => b.kinhNghiem)
    .Include(b => b.chuyenNganh)
    .Include(b => b.applicationUser)
    .ToListAsync();

            return applicationDbContext;
        }

        public async Task<BaiTuyenDung> GetByIdAsync(int id)
        {
            var applicationDbContext = await _context.baiTuyenDungs
    .Include(b => b.thanhPho)
    .Include(b => b.kinhNghiem)
    .Include(b => b.chuyenNganh)
    .Include(b => b.applicationUser)
    .ToListAsync();
            return await _context.baiTuyenDungs.FindAsync(id);
        }

        public async Task AddAsync(BaiTuyenDung baiTuyenDung)
        {
            _context.baiTuyenDungs.Add(baiTuyenDung);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BaiTuyenDung baiTuyenDung)
        {
            _context.baiTuyenDungs.Update(baiTuyenDung);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var baiTuyenDung = await _context.baiTuyenDungs.FindAsync(id);
            if (baiTuyenDung != null)
            {
                _context.baiTuyenDungs.Remove(baiTuyenDung);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<BaiTuyenDung>> GetBaiTuyenDungByUserIdAsync(string userId)
        {
            var baiTuyenDungByUserId = await _context.baiTuyenDungs
                .Where(b => b.applicationUser.Id == userId && b.TrangThai == true)
                .Include(b => b.thanhPho)
                .Include(b => b.kinhNghiem)
                .Include(b => b.chuyenNganh)
                .Include(b => b.applicationUser)
                .ToListAsync();

            return baiTuyenDungByUserId;
        }
        public async Task<int> CountBaiTuyenDungAsync()
        {
            return await _context.baiTuyenDungs.CountAsync();
        }
        public async Task<int> CountBaiTuyenDungTodayAsync()
        {
            var today = DateTime.Today;

            var count = await _context.baiTuyenDungs
                .Where(b => b.ThoiGianDangBai.Value == today)
                .CountAsync();

            return count;
        }
        public async Task<int> CountBaiTuyenDungByMajorAsync(string majorName)
        {
            var count = await _context.baiTuyenDungs
                .Join(
                    _context.baiTuyenDung_ChuyenNganhs,
                    baiTuyenDung => baiTuyenDung.BaiTuyenDung_id,
                    baiTuyenDung_ChuyenNganh => baiTuyenDung_ChuyenNganh.BaiTuyenDungid,
                    (baiTuyenDung, baiTuyenDung_ChuyenNganh) => new { baiTuyenDung, baiTuyenDung_ChuyenNganh }
                )
                .Join(
                    _context.chuyenNganhs,
                    joined => joined.baiTuyenDung_ChuyenNganh.ChuyenNganhid,
                    chuyenNganh => chuyenNganh.ChuyenNganh_id,
                    (joined, chuyenNganh) => new { joined.baiTuyenDung, chuyenNganh }
                )
                .Where(joined => joined.chuyenNganh.ChuyenNganh_name == majorName)
                .CountAsync();

            return count;
        }
    }
}
