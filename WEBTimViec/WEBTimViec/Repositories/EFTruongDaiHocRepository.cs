using Microsoft.EntityFrameworkCore;
using WEBTimViec.Data;
using WEBTimViec.Models;
namespace WEBTimViec.Repositories
{
    public class EFTruongDaiHocRepository : ITruongDaiHoc
    {
        private readonly ApplicationDbContext _context;
        public EFTruongDaiHocRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TruongDaiHoc>> GetAllAsync()
        {
            return await _context.truongDaiHocs.ToListAsync();
        }
    }
}
