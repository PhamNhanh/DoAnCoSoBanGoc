using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEBTimViec.Data;
using WEBTimViec.Models;
using WEBTimViec.Repositories;

namespace WEBTimViec.Areas.UngVien.UngVien
{
    [Area("UngVien")]
    [Authorize(Roles = SD.Role_User)]
    public class UVController : Controller
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

        public UVController(ApplicationDbContext context,
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
            var applicationDbContext = _context.baiTuyenDungs.Include(b => b.KyNangMem).Include(b => b.kinhNghiem).Include(b => b.thanhPho);
            return View(await applicationDbContext.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> DSUngTuyen()
        {
            var find_user = await _userManager.GetUserAsync(User);
            if (find_user != null)
            {

                var dsUngTuyen = await _ungTuyen.GetAllApplyByUserIdAsync(find_user.Id);
                return View(dsUngTuyen);
            }
            else
            {
                return NotFound();
            }

        }
        public async Task<IActionResult> DetailsBaiTuyenDung(int id)
        {
            var baiTuyenDung = await _baiTuyenDung.GetByIdAsync(id);
            ViewData["TenTP"] = await _thanhPho.HienThiTenTP(baiTuyenDung.thanhPhoId);
            return View(baiTuyenDung);
        }
        public async Task<IActionResult> ListBaiTuyenDung()
        {
            var baiTuyenDung = await _baiTuyenDung.GetAllAsync();
            return View(baiTuyenDung);
        }
        [HttpGet]
        public async Task<IActionResult> UngTuyen(int Id)
        {
            var baiTuyenDung = await _baiTuyenDung.GetByIdAsync(Id);
            ViewBag.BaiTuyenDung = baiTuyenDung;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UngTuyen(UngTuyen ungTuyen, IFormFile url_cv, int id)
        {
            var find_user = await _userManager.GetUserAsync(User);
            if (find_user == null)
            {
                return View("Thiếu Id user");
            }
            var post = await _baiTuyenDung.GetByIdAsync(id);
            if (post == null)
            {
                return View("Thiếu Id postjob");
            }

            if (ModelState.IsValid)
            {
                if (url_cv != null && IsFileSizeValid(url_cv))
                {
                    // Lưu CV
                    ungTuyen.url_CV = await SaveCV(url_cv);
                }
                else
                {
                    ModelState.AddModelError("url_cv", "Vui lòng chọn một tệp pdf hợp lệ và có kích thước nhỏ hơn 10MB.");
                    return View(ungTuyen);
                }
                // Đảm bảo rằng đường dẫn CV đã được lưu một cách chính xác và hợp lệ
                if (string.IsNullOrEmpty(ungTuyen.url_CV))
                {
                    ModelState.AddModelError("url_cv", "Đường dẫn CV không hợp lệ.");
                    return View(ungTuyen);
                }

                ungTuyen.BaiTuyenDungid = post.BaiTuyenDung_id;
                ungTuyen.applicationUser = post.applicationUser; // Đã lưu id Nhà Tuyển Dụng
                ungTuyen.application_Userid = find_user.Id; //Chưa lưuu id Ung Viên
                ungTuyen.ThoiGianUngTuyen = DateTime.Now;

                await _ungTuyen.AddAsync(ungTuyen);
                return RedirectToAction(nameof(DSUngTuyen));
            }
            else
            {
                // ModelState.IsValid không đúng, xử lý hoặc trả về lỗi
                return BadRequest("Dữ liệu không hợp lệ.");
            }
        }


        private async Task<string> SaveCV(IFormFile url_cv)
        {
            try
            {
                //đảm bảo tên cv là duy nhất khi up
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(url_cv.FileName);
                var savePath = Path.Combine("wwwroot/filecv", fileName); // Thay đổi đường dẫn theo cấu hình của bạn
                using (var fileStream = new FileStream(savePath, FileMode.Create))
                {
                    await url_cv.CopyToAsync(fileStream);
                }
                return "/filecv/" + fileName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        private bool IsFileSizeValid(IFormFile file)
        {
            // Kiểm tra kích thước file không vượt quá 10MB
            var maxSize = 10 * 1024 * 1024; // 10MB
            return file.Length <= maxSize;
        }

    }
}
