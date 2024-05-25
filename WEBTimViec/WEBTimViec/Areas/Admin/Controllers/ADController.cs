using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBTimViec.Data;
using WEBTimViec.Models;
using WEBTimViec.Repositories;

namespace WEBTimViec.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ADController : Controller
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

        public ADController(ApplicationDbContext context,
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

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.baiTuyenDungs.Include(b => b.kinhNghiem).Include(b => b.thanhPho);
            return View(await applicationDbContext.ToListAsync());
        }

        //Thanh Pho

        [HttpGet]
        public IActionResult AddThanhPho()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddThanhPho(ThanhPho thanhPho)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(thanhPho.ThanhPho_name))
                {
                    TempData["ErrorMessage"] = "Vui lòng nhập thông tin đầy đủ";
                    return View(thanhPho);
                }

                // Nếu không có lỗi, thêm thể loại và hiển thị thông báo thành công
                await _thanhPho.AddAsync(thanhPho);
                TempData["SuccessMessage"] = "Đã thêm thể loại thành công";
                return RedirectToAction("IndexThanhPho", "Home");
            }
            return View(thanhPho);
        }
        public async Task<IActionResult> ListThanhPho()
        {
            var thanhPho = await _thanhPho.GetAllAsync();
            var sortedThanhPho = thanhPho.OrderBy(tp => tp.ThanhPho_name).ToList();
            return View(sortedThanhPho);
        }


        //Chuyen Nganh
        [HttpGet]
        public IActionResult AddChuyenNganh()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddChuyenNganh(ChuyenNganh chuyenNganh)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(chuyenNganh.ChuyenNganh_name))
                {
                    TempData["ErrorMessage"] = "Vui lòng nhập thông tin đầy đủ";
                    return View(chuyenNganh);
                }

                // Nếu không có lỗi, thêm thể loại và hiển thị thông báo thành công
                await _chuyenNganh.AddAsync(chuyenNganh);
                TempData["SuccessMessage"] = "Đã thêm thể loại thành công";
                return RedirectToAction("IndexChuyenNganh", "Home");
            }
            return View(chuyenNganh);
        }
        public async Task<IActionResult> ListChuyenNganh()
        {
            var chuyenNganh = await _chuyenNganh.GetAllAsync();

            return View(chuyenNganh);
        }



    }
}
