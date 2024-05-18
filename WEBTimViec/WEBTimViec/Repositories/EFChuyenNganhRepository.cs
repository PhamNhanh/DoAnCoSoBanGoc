using static WEBTimViec.Repositories.EFChuyenNganhRepository;
using WEBTimViec.Data;
using WEBTimViec.Models;
using Microsoft.EntityFrameworkCore;

namespace WEBTimViec.Repositories
{
    public class EFChuyenNganhRepository : IChuyenNganh
    {
            private readonly ApplicationDbContext _context;
            public EFChuyenNganhRepository(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<ChuyenNganh>> GetAllAsync()
            {
                return await _context.chuyenNganhs.ToListAsync();
            }
        public async Task AddAsync(ChuyenNganh chuyenNganh)
        {
            _context.chuyenNganhs.Add(chuyenNganh);
            await _context.SaveChangesAsync();
        }
    }
}
