using Microsoft.EntityFrameworkCore;
using WEBTimViec.Models;  // Đảm bảo rằng bạn có model LoaiTaiKhoan trong thư mục Models
using WEBTimViec.Data;   // Đảm bảo rằng bạn có ApplicationDbContext trong thư mục Data

namespace WEBTimViec.Repositories
{
    public class EFLoaiTaiKhoan : ILoaiTaiKhoan  // Triển khai interface ILoaiTaiKhoan
    {
        private readonly ApplicationDbContext _context;

        public EFLoaiTaiKhoan(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả các loại tài khoản
        public async Task<List<LoaiTaiKhoan>> GetAllLoaiTaiKhoanAsync()
        {
            return await _context.LoaiTaiKhoans.ToListAsync();
        }

        // Lấy loại tài khoản theo ID
        public async Task<LoaiTaiKhoan> GetLoaiTaiKhoanByIdAsync(int id)
        {
            return await _context.LoaiTaiKhoans
                                 .FirstOrDefaultAsync(l => l.loaiTaiKhoanId == id);
        }

        // Thêm loại tài khoản mới
        public async Task AddLoaiTaiKhoanAsync(LoaiTaiKhoan loaiTaiKhoan)
        {
            await _context.LoaiTaiKhoans.AddAsync(loaiTaiKhoan);
            await _context.SaveChangesAsync();
        }

        // Cập nhật loại tài khoản
        public async Task UpdateLoaiTaiKhoanAsync(LoaiTaiKhoan loaiTaiKhoan)
        {
            _context.LoaiTaiKhoans.Update(loaiTaiKhoan);
            await _context.SaveChangesAsync();
        }

        // Xóa loại tài khoản
        public async Task DeleteLoaiTaiKhoanAsync(int id)
        {
            var loaiTaiKhoan = await _context.LoaiTaiKhoans
                                               .FirstOrDefaultAsync(l => l.loaiTaiKhoanId == id);
            if (loaiTaiKhoan != null)
            {
                _context.LoaiTaiKhoans.Remove(loaiTaiKhoan);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<string> GetTenLoaiTaiKhoanByUserIdAsync(string userId)
        {
            var user = await _context.Users
                                      .FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                var loaiTaiKhoan = await _context.LoaiTaiKhoans
                                                  .FirstOrDefaultAsync(l => l.loaiTaiKhoanId == user.loaiTaiKhoanId);

                return loaiTaiKhoan != null ? loaiTaiKhoan.tenLoaiTaiKhoan : "Chưa xác định loại tài khoản";
            }

            return "User không tồn tại";
        }

    }
}
