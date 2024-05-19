using static WEBTimViec.Repositories.EFViTriCongViecRepository;
using WEBTimViec.Data;
using WEBTimViec.Models;
using Microsoft.EntityFrameworkCore;

namespace WEBTimViec.Repositories
{
    public class EFViTriCongViecRepository : IViTriCongViec
    {
        private readonly ApplicationDbContext _context;
        public EFViTriCongViecRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ViTriCongViec>> GetAllAsync()
        {
            return await _context.viTriCongViecs.ToListAsync();
        }
        public async Task AddAsync(ViTriCongViec viTriCongViec)
        {
            _context.viTriCongViecs.Add(viTriCongViec);
            await _context.SaveChangesAsync();
        }
    }
}
