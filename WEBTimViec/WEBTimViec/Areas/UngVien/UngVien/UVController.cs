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
            var applicationDbContext = _context.baiTuyenDungs.Include(b => b.kinhNghiem).Include(b => b.thanhPho);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> DSNhaTuyenDung()
        {
            // Lấy danh sách các nhà tuyển dụng từ nguồn dữ liệu (ví dụ: database)
            var danhSachNhaTuyenDung = await _userRepository.GetAllCompanyAsync();
            // Truyền danh sách nhà tuyển dụng tới view
            return View(danhSachNhaTuyenDung);
        }

        public async Task<IActionResult> DetailsNhaTuyenDung(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Users.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
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
            {
                var baiTuyenDung = _context.baiTuyenDungs
            .Include(b => b.baiTuyenDung_ChuyenNganhs)
                .ThenInclude(bcn => bcn.chuyenNganh)
            .Include(b => b.baiTuyenDung_ViTris)
                .ThenInclude(bv => bv.viTriCongViec)
            .Include(b => b.baiTuyenDung_KyNangMems)
                .ThenInclude(bk => bk.kyNangMem)
            .FirstOrDefault(b => b.BaiTuyenDung_id == id);

                if (baiTuyenDung == null)
                {
                    return NotFound();
                }

                ViewBag.TenTP = _context.thanhPhos
                    .Where(tp => tp.ThanhPho_id == baiTuyenDung.thanhPhoId)
                    .Select(tp => tp.ThanhPho_name)
                    .FirstOrDefault();

                // Lấy danh sách chuyên ngành dựa trên BaiTuyenDungId
                ViewBag.ChuyenNganhs = baiTuyenDung.baiTuyenDung_ChuyenNganhs
                    .Where(bcn => bcn.BaiTuyenDungid == id)
                    .Select(bcn => bcn.chuyenNganh.ChuyenNganh_name)
                    .ToList();

                // Lấy danh sách vị trí công việc dựa trên BaiTuyenDungId
                ViewBag.ViTriCongViecs = baiTuyenDung.baiTuyenDung_ViTris
                    .Where(bv => bv.BaiTuyenDungid == id)
                    .Select(bv => bv.viTriCongViec.ViTriCongViec_name)
                    .ToList();

                // Lấy danh sách kỹ năng mềm dựa trên BaiTuyenDungId
                ViewBag.KyNangMems = baiTuyenDung.baiTuyenDung_KyNangMems
                    .Where(bk => bk.BaiTuyenDungid == id)
                    .Select(bk => bk.kyNangMem.KNMem_name)
                    .ToList();

                return View(baiTuyenDung);
            }
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
            //id người dùng
            var find_user = await _userManager.GetUserAsync(User);
            //id bài tuyển dụng
            var post = await _baiTuyenDung.GetByIdAsync(id);


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
                ungTuyen.application_Userid = find_user.Id; //Da lưuu id Ung Viên
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
