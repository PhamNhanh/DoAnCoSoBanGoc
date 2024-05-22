using static WEBTimViec.Repositories.EFKyNangMemRepository;
using WEBTimViec.Data;
using WEBTimViec.Models;
using Microsoft.EntityFrameworkCore;

namespace WEBTimViec.Repositories
{
    public class EFKyNangMemRepository : IKyNangMem
    {
            private readonly ApplicationDbContext _context;
            public EFKyNangMemRepository(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<KyNangMem>> GetAllAsync()
            {
                return await _context.kyNangMems.ToListAsync();
            }
        public async Task AddAsync(KyNangMem kyNangMem)
        {
            _context.kyNangMems.Add(kyNangMem);
            await _context.SaveChangesAsync();
        }
    }
}
