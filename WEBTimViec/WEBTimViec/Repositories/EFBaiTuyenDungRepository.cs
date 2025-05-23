﻿using Microsoft.EntityFrameworkCore;
using WEBTimViec.Data;
using WEBTimViec.Models;

namespace WEBTimViec.Repositories
{
    public class EFBaiTuyenDungRepository : IBaiTuyenDung
    {
        private readonly ApplicationDbContext _context;
        public EFBaiTuyenDungRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BaiTuyenDung>> GetAllAsync()
        {
            //bao gồm danh mục, nếu không có sẽ ko ra danh mục
            var applicationDbContext = await _context.baiTuyenDungs
                 .Include(b => b.TenBaiTuyenDung)
                .Include(b => b.MoTaCongViec)
                .Include(b => b.YeuCauKyNang)
                .Include(b => b.PhucLoi)
                .Include(b => b.Luong_min)
                .Include(b => b.Luong_max)
                .Include(b => b.KieuCongViec)
                .Include(b => b.ThoiGianDangBai)
                .Include(b => b.ThoiGianCapNhat)
                .ToListAsync();

            return await _context.baiTuyenDungs.ToListAsync();
        }

        public async Task<IEnumerable<BaiTuyenDung>> GetAllByCompanyIdAsync(string id)
        {
            //bao gồm danh mục, nếu không có sẽ ko ra danh mục
            var applicationDbContext = await _context.baiTuyenDungs
                 .Include(b => b.TenBaiTuyenDung)
                .Include(b => b.MoTaCongViec)
                .Include(b => b.YeuCauKyNang)
                .Include(b => b.PhucLoi)
                .Include(b => b.Luong_min)
                .Include(b => b.Luong_max)
                .Include(b => b.KieuCongViec)
                .Include(b => b.ThoiGianDangBai)
                .Include(b => b.ThoiGianCapNhat)
                .ToListAsync();

            return applicationDbContext;
        }


        public async Task<BaiTuyenDung> GetByIdAsync(int id)
        {
            var applicationDbContext = await _context.baiTuyenDungs
                .Include(b => b.TenBaiTuyenDung)
                .Include(b => b.MoTaCongViec)
                .Include(b => b.YeuCauKyNang)
                .Include(b => b.PhucLoi)
                .Include(b => b.Luong_min)
                .Include(b => b.Luong_max)
                .Include(b => b.KieuCongViec)
                .Include(b => b.ThoiGianDangBai)
                .Include(b => b.ThoiGianCapNhat)
                .ToListAsync();
            return await _context.baiTuyenDungs.FindAsync(id);
        }

        public async Task AddAsync(BaiTuyenDung baiTuyenDung)
        {
            _context.baiTuyenDungs.Add(baiTuyenDung);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BaiTuyenDung baiTuyenDung)
        {
            _context.baiTuyenDungs.Update(baiTuyenDung);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var baiTuyenDung = await _context.baiTuyenDungs.FindAsync(id);
            if (baiTuyenDung != null)
            {
                _context.baiTuyenDungs.Remove(baiTuyenDung);
                await _context.SaveChangesAsync();
            }
        }
    }
}
