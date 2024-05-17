using Microsoft.EntityFrameworkCore;
using WEBTimViec.Data;
using WEBTimViec.Models;

namespace WEBTimViec.Repositories
{
    public class EFHocVanRepository : IHocVan
    {
        private readonly ApplicationDbContext _context;
        public EFHocVanRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<HocVan>> GetAllAsync()
        {
            //bao gồm danh mục, nếu không có sẽ ko ra danh mục
            var applicationDbContext = await _context.hocVans
                .Include(b => b.GPA)
                .Include(b => b.NgayBatDau)
                .Include(b => b.NgayTotNghiep)
                .ToListAsync();

            return await _context.hocVans.ToListAsync();
        }
        public async Task<HocVan> GetByIdAsync(int id)
        {
            var applicationDbContext = await _context.hocVans
                .Include(b => b.GPA)
                .Include(b => b.NgayBatDau)
                .Include(b => b.NgayTotNghiep)
                .ToListAsync();
            return await _context.hocVans.FindAsync(id);
        }


        public async Task<IEnumerable<HocVan>> GetByIdUserAsync(string id)
        {
            var educations = await _context.hocVans
                .Include(b => b.GPA)
                .Include(b => b.NgayBatDau)
                .Include(b => b.NgayTotNghiep)
                .Where(b => b.applicationUser.Id == id)
                .ToListAsync();

            return educations;
        }



        public async Task AddAsync(HocVan education)
        {
            _context.hocVans.Add(education);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(HocVan education)
        {
            _context.hocVans.Update(education);
            await _context.SaveChangesAsync();
        }

        public async Task? DeleteAsync(int id)
        {
            var education = await _context.hocVans.FindAsync(id);
            _context.hocVans.Remove(education);
            await _context.SaveChangesAsync();
        }
    }
}
