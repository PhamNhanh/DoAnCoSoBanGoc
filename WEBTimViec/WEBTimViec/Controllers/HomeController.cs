using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WEBTimViec.Data;
using WEBTimViec.Models;
using WEBTimViec.Repositories;
using System.Linq;

namespace WEBTimViec.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBaiTuyenDung _baiTuyenDung;
        private readonly IChuyenNganh _chuyenNganh;
        private readonly IThanhPho _thanhPho;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IKinhNghiem _kinhNghiem;
        private readonly IUngTuyen _ungTuyen;
        private readonly IViTriCongViec _viTriCongViec;
        public HomeController(ApplicationDbContext context,
            IBaiTuyenDung baiTuyenDung,
            IChuyenNganh chuyenNganh,
            IThanhPho thanhPho,
            IKinhNghiem kinhNghiem,
            UserManager<ApplicationUser> userManager,
            IUngTuyen ungTuyen,
            IViTriCongViec viTriCongViec)
        {
            _context = context;
            _baiTuyenDung = baiTuyenDung;
            _chuyenNganh = chuyenNganh;
            _kinhNghiem = kinhNghiem;
            _thanhPho = thanhPho;
            _userManager = userManager;
            _ungTuyen = ungTuyen;
            _viTriCongViec = viTriCongViec;
        }
        public IActionResult Index()
        {
/*            var hienthi_NhaTuyenDung = await _userManager.GetUserAsync(User);*/

            return View();
        }
        public async Task<IActionResult> AddBaiTuyenDung()
        {
            var kinhNghiem = await _kinhNghiem.GetAllAsync();
            var sortedKinhNghiem = kinhNghiem.OrderBy(kn => kn.NamKinhNghiem).ToList();

            var chuyenNganh = await _chuyenNganh.GetAllAsync();
            var sortedChuyenNganh = chuyenNganh.OrderBy(cn => cn.ChuyenNganh_name).ToList();

            var thanhPho = await _thanhPho.GetAllAsync();
            var sortedThanhPho = thanhPho.OrderBy(tp => tp.ThanhPho_name).ToList();

            var viTriCongViec = await _viTriCongViec.GetAllAsync();
            var sortedVitricongviec = viTriCongViec.OrderBy(tp => tp.ViTriCongViec_name).ToList();

            ViewBag.KinhNghiem = new SelectList(sortedKinhNghiem, "KinhNghiem_id", "NamKinhNghiem");
            ViewBag.ChuyenNganh = new SelectList(sortedChuyenNganh, "ChuyenNganh_id", "ChuyenNganh_name");
            ViewBag.ThanhPho = new SelectList(sortedThanhPho, "ThanhPho_id", "ThanhPho_name");
            ViewBag.ViTriCongViec = new SelectList(sortedVitricongviec, "ViTriCongViec_id", "ViTriCongViec_name");

            return View();
        }
        public async Task<IActionResult> Create([Bind("BaiTuyenDung_id,TenCongViec,MoTaCongViec,YeuCauKyNang,PhucLoi,KieuCongViec,Luong_min,Luong_max,thanhPhoid,ThoiGianDangBai,ThoiGianCapNhat,kinhNghiemid,chuyenNganhid,applicationUserid")] BaiTuyenDung baiTuyenDung)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(baiTuyenDung.TenCongViec))
                {
                    ModelState.AddModelError(nameof(baiTuyenDung.TenCongViec), "Tên công việc không được để trống.");
                    return View(baiTuyenDung);
                }
                if (!User.Identity.IsAuthenticated)
                {
                    // Chưa đăng nhập, chuyển hướng tới trang đăng nhập
                    return Redirect("/Identity/Account/Login");
                }
                if (baiTuyenDung.ThoiGianHetHan <= DateTime.Now)
                {
                    ModelState.AddModelError(nameof(baiTuyenDung.ThoiGianHetHan), "Thời gian hết hạn phải lớn hơn thời gian hiện tại.");
                    return View(baiTuyenDung);
                }
                var find_company = await _userManager.GetUserAsync(User);
                if (find_company != null)
                    baiTuyenDung.applicationUser = find_company;

                baiTuyenDung.ThoiGianDangBai = DateTime.Now;
                baiTuyenDung.ThoiGianHetHan = baiTuyenDung.ThoiGianHetHan;
                baiTuyenDung.YeuCauKyNang = baiTuyenDung.YeuCauKyNang?.Replace("\r\n", "\n");
                baiTuyenDung.MoTaCongViec = baiTuyenDung.MoTaCongViec?.Replace("\r\n", "\n");
                baiTuyenDung.PhucLoi = baiTuyenDung.PhucLoi?.Replace("\r\n", "\n");
                if (baiTuyenDung.Luong_min < 0)
                {
                    ModelState.AddModelError(nameof(baiTuyenDung.Luong_min), "Lương từ không được âm.");
                    return Redirect("/Home/AddBaiTuyenDung");
                }

                if (baiTuyenDung.Luong_min >= baiTuyenDung.Luong_max)
                {
                    ModelState.AddModelError(nameof(baiTuyenDung.Luong_max), "Lương từ phải nhỏ hơn lương đến.");
                    return Redirect("/Home/AddBaiTuyenDung");
                }

                await _baiTuyenDung.AddAsync(baiTuyenDung);
                await _context.SaveChangesAsync();
                return RedirectToAction("IndexBaiTuyenDung", "Home");
            }
            return View(baiTuyenDung);
        }

        public async Task<IActionResult> IndexBaiTuyenDung()
        {
            var baiTuyenDung = await _baiTuyenDung.GetAllAsync();
            return View(baiTuyenDung);
        }
        public async Task<IActionResult> DetailsBaiTuyenDung(int id)
        {
            var baiTuyenDung = await _baiTuyenDung.GetByIdAsync(id);
            return View(baiTuyenDung);
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
        public async Task<IActionResult> IndexThanhPho()
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
        public async Task<IActionResult> IndexChuyenNganh()
        {
            var chuyenNganh = await _chuyenNganh.GetAllAsync();

            return View(chuyenNganh);
        }




        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName); //

            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName; // Trả về đường dẫn tương đối
        }
        
    }
}
