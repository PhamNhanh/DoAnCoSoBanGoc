using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEBTimViec.Data;
using WEBTimViec.Models;
using WEBTimViec.Repositories;

namespace WEBTimViec.Areas.NhaTuyenDung.Controllers
{
    [Area("NhaTuyenDung")]
    [Authorize(Roles = SD.Role_Company)]
    public class BaiTuyenDungController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBaiTuyenDung _baiTuyenDung;
        private readonly IChuyenNganh _chuyenNganh;
        private readonly IThanhPho _thanhPho;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IKinhNghiem _kinhNghiem;
        private readonly IUngTuyen _ungTuyen;
        private readonly IViTriCongViec _viTriCongViec;
        private readonly IUserRepository _userRepository;
        private readonly IKyNangMem _kyNangMem;

        public BaiTuyenDungController(ApplicationDbContext context,
            IBaiTuyenDung baiTuyenDung,
            IChuyenNganh chuyenNganh,
            IThanhPho thanhPho,
            IKinhNghiem kinhNghiem,
            UserManager<ApplicationUser> userManager,
            IUngTuyen ungTuyen,
            IUserRepository userRepository,
            IViTriCongViec viTriCongViec,
            IKyNangMem kyNangMem)
        {
            _context = context;
            _baiTuyenDung = baiTuyenDung;
            _chuyenNganh = chuyenNganh;
            _kinhNghiem = kinhNghiem;
            _thanhPho = thanhPho;
            _userManager = userManager;
            _ungTuyen = ungTuyen;
            _userRepository = userRepository;
            _viTriCongViec = viTriCongViec;
            _kyNangMem = kyNangMem;
        }

        // GET: NhaTuyenDung/BaiTuyenDung
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.baiTuyenDungs.Include(b => b.KyNangMem).Include(b => b.kinhNghiem).Include(b => b.thanhPho);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: NhaTuyenDung/BaiTuyenDung/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baiTuyenDung = await _context.baiTuyenDungs
                .Include(b => b.KyNangMem)
                .Include(b => b.kinhNghiem)
                .Include(b => b.thanhPho)
                .FirstOrDefaultAsync(m => m.BaiTuyenDung_id == id);
            if (baiTuyenDung == null)
            {
                return NotFound();
            }

            return View(baiTuyenDung);
        }

        // GET: NhaTuyenDung/BaiTuyenDung/Create
        public IActionResult Create()
        {
            ViewData["KyNangMemId"] = new SelectList(_context.kyNangMems, "KyNangMem_id", "KyNangMem_id");
            ViewData["kinhNghiemId"] = new SelectList(_context.kinhNghiems, "KinhNghiem_id", "KinhNghiem_id");
            ViewData["thanhPhoId"] = new SelectList(_context.thanhPhos, "ThanhPho_id", "ThanhPho_id");
            return View();
        }

        // POST: NhaTuyenDung/BaiTuyenDung/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BaiTuyenDung_id,TenCongViec,MoTaCongViec,YeuCauKyNang,PhucLoi,Luong_min,Luong_max,KieuCongViec,KyNangMemId,ThoiGianDangBai,ThoiGianHetHan,thanhPhoId,kinhNghiemId")] BaiTuyenDung baiTuyenDung)
        {
            if (ModelState.IsValid)
            {
                _context.Add(baiTuyenDung);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KyNangMemId"] = new SelectList(_context.kyNangMems, "KyNangMem_id", "KyNangMem_id", baiTuyenDung.KyNangMemId);
            ViewData["kinhNghiemId"] = new SelectList(_context.kinhNghiems, "KinhNghiem_id", "KinhNghiem_id", baiTuyenDung.kinhNghiemId);
            ViewData["thanhPhoId"] = new SelectList(_context.thanhPhos, "ThanhPho_id", "ThanhPho_id", baiTuyenDung.thanhPhoId);
            return View(baiTuyenDung);
        }

        // GET: NhaTuyenDung/BaiTuyenDung/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baiTuyenDung = await _context.baiTuyenDungs.FindAsync(id);
            if (baiTuyenDung == null)
            {
                return NotFound();
            }
            ViewData["KyNangMemId"] = new SelectList(_context.kyNangMems, "KyNangMem_id", "KyNangMem_id", baiTuyenDung.KyNangMemId);
            ViewData["kinhNghiemId"] = new SelectList(_context.kinhNghiems, "KinhNghiem_id", "KinhNghiem_id", baiTuyenDung.kinhNghiemId);
            ViewData["thanhPhoId"] = new SelectList(_context.thanhPhos, "ThanhPho_id", "ThanhPho_id", baiTuyenDung.thanhPhoId);
            return View(baiTuyenDung);
        }

        // POST: NhaTuyenDung/BaiTuyenDung/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BaiTuyenDung_id,TenCongViec,MoTaCongViec,YeuCauKyNang,PhucLoi,Luong_min,Luong_max,KieuCongViec,KyNangMemId,ThoiGianDangBai,ThoiGianHetHan,thanhPhoId,kinhNghiemId")] BaiTuyenDung baiTuyenDung)
        {
            if (id != baiTuyenDung.BaiTuyenDung_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(baiTuyenDung);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BaiTuyenDungExists(baiTuyenDung.BaiTuyenDung_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["KyNangMemId"] = new SelectList(_context.kyNangMems, "KyNangMem_id", "KyNangMem_id", baiTuyenDung.KyNangMemId);
            ViewData["kinhNghiemId"] = new SelectList(_context.kinhNghiems, "KinhNghiem_id", "KinhNghiem_id", baiTuyenDung.kinhNghiemId);
            ViewData["thanhPhoId"] = new SelectList(_context.thanhPhos, "ThanhPho_id", "ThanhPho_id", baiTuyenDung.thanhPhoId);
            return View(baiTuyenDung);
        }

        // GET: NhaTuyenDung/BaiTuyenDung/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var baiTuyenDung = await _context.baiTuyenDungs
                .Include(b => b.KyNangMem)
                .Include(b => b.kinhNghiem)
                .Include(b => b.thanhPho)
                .FirstOrDefaultAsync(m => m.BaiTuyenDung_id == id);
            if (baiTuyenDung == null)
            {
                return NotFound();
            }

            return View(baiTuyenDung);
        }

        // POST: NhaTuyenDung/BaiTuyenDung/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var baiTuyenDung = await _context.baiTuyenDungs.FindAsync(id);
            if (baiTuyenDung != null)
            {
                _context.baiTuyenDungs.Remove(baiTuyenDung);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BaiTuyenDungExists(int id)
        {
            return _context.baiTuyenDungs.Any(e => e.BaiTuyenDung_id == id);
        }
    }
}
