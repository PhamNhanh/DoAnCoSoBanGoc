using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            return await _context.hocVans.ToListAsync();
        }

        public async Task<HocVan> GetByIdAsync(int id)
        {
            return await _context.hocVans.FindAsync(id);
        }

        public async Task AddAsync(HocVan hocVan)
        {
            _context.hocVans.Add(hocVan);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(HocVan hocVan)
        {
            _context.hocVans.Update(hocVan);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var hocVan = await _context.hocVans.FindAsync(id);
            if (hocVan != null)
            {
                _context.hocVans.Remove(hocVan);
                await _context.SaveChangesAsync();
            }
        }
            public async Task<IEnumerable<HocVan>> GetByIdUserAsync(string id)
            {
                return await _context.hocVans
                    .Where(h => h.applicationUser.Id == id)
                    .ToListAsync();
            }
    }
}
