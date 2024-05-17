using WEBTimViec.Data;
using WEBTimViec.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WEBTimViec.Repositories
{
    public class EFKinhNghiemRepository : IKinhNghiem
    {
        private readonly ApplicationDbContext _context;

        public EFKinhNghiemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<KinhNghiem>> GetAllAsync()
        {
            return await _context.kinhNghiems.ToListAsync();
        }
    }
}
