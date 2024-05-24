using Microsoft.EntityFrameworkCore;
using WEBTimViec.Data;
using WEBTimViec.Models;

namespace WEBTimViec.Repositories
{
    public class EFThanhPhoRepository : IThanhPho
    {

        private readonly ApplicationDbContext _context;
        public EFThanhPhoRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ThanhPho>> GetAllAsync()
        {
            // return await _context.Products.ToListAsync();
            return await _context.thanhPhos
            .ToListAsync();
        }


        public async Task<ThanhPho> GetByIdAsync(int id)
        {
            return await _context.thanhPhos.FindAsync(id);
        }



        public async Task AddAsync(ThanhPho thanhnPho)
        {
            _context.thanhPhos.Add(thanhnPho);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ThanhPho thanhPho)
        {
            _context.thanhPhos.Update(thanhPho);
            await _context.SaveChangesAsync();
        }

        public async Task? DeleteAsync(int id)
        {
            var thanhPho = await _context.hocVans.FindAsync(id);
            _context.hocVans.Remove(thanhPho);
            await _context.SaveChangesAsync();
        }
        public async Task<string> HienThiTenTP(int? Id)
        {

            var TenTP = await _context.thanhPhos
                .Where(tp => tp.ThanhPho_id == Id)
                .Select(tp => tp.ThanhPho_name)
                .FirstOrDefaultAsync();

            return TenTP;
        }
    }
}
