using Microsoft.EntityFrameworkCore;
using WEBTimViec.Data;
using WEBTimViec.Models;
namespace WEBTimViec.Repositories
{
    public class EFBaiTuyenDung_ChuyenNganh : IBaiTuyenDung_ChuyenNganh
    {
        private readonly ApplicationDbContext _context;

        public EFBaiTuyenDung_ChuyenNganh(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BaiTuyenDung_ChuyenNganh>> GetAllAsync()
        {
            return await _context.baiTuyenDung_ChuyenNganhs.ToListAsync();
        }

        public async Task<IEnumerable<ChuyenNganh>> GetAllChuyenNganhAsync()
        {
            return await _context.chuyenNganhs.ToListAsync();
        }

        Task<IEnumerable<ChuyenNganh>> IBaiTuyenDung_ChuyenNganh.GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
