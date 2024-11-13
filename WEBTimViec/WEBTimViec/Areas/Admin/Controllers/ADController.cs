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
            // Lấy danh sách bài tuyển dụng có trạng thái là true và thông tin người dùng
            var baiTuyenDungs = await _context.baiTuyenDungs
                .Where(b => b.TrangThai == true)
                .Include(b => b.applicationUser)  // Tải thông tin người dùng
                .ToListAsync();

            // Lấy danh sách thành phố và sắp xếp theo tên
            var thanhPho = await _thanhPho.GetAllAsync();
            var sortedThanhPho = thanhPho.OrderBy(tp => tp.ThanhPho_name).ToList();

            // Lấy danh sách chuyên ngành và sắp xếp theo tên
            var chuyenNganh = await _chuyenNganh.GetAllAsync();
            var sortedChuyenNganh = chuyenNganh.OrderBy(cn => cn.ChuyenNganh_name).ToList();

            // Tạo một danh sách để lưu số lượng bài tuyển dụng theo chuyên ngành
            var jobCountsByMajor = new List<MajorViewModel>();

            // Đếm số lượng bài tuyển dụng cho mỗi chuyên ngành
            foreach (var chuyenNganhItem in sortedChuyenNganh)
            {
                var count = await _context.baiTuyenDung_ChuyenNganhs
                    .Where(bcn => bcn.ChuyenNganhid == chuyenNganhItem.ChuyenNganh_id)
                    .CountAsync();

                if (count > 0) // Loại bỏ những chuyên ngành không có bài tuyển dụng
                {
                    jobCountsByMajor.Add(new MajorViewModel
                    {
                        ChuyenNganhName = chuyenNganhItem.ChuyenNganh_name,
                        JobCount = count
                    });
                }
            }

            // Sắp xếp các chuyên ngành theo số lượng bài tuyển dụng giảm dần
            jobCountsByMajor = jobCountsByMajor.OrderByDescending(m => m.JobCount).ToList();

            // Tạo ViewModel để truyền dữ liệu sang View
            var viewModel = new ViewModel
            {
                BaiTuyenDungs = baiTuyenDungs,
                ThanhPhos = sortedThanhPho,
                ChuyenNganhs = sortedChuyenNganh,
                ApplicationUsers = baiTuyenDungs.Select(b => b.applicationUser).ToList(), // Lấy danh sách người dùng từ bài tuyển dụng
                Majors = jobCountsByMajor // Thêm danh sách chuyên ngành được sắp xếp với số lượng bài tuyển dụng
            };

            return View(viewModel);
        }

        public async Task<IActionResult> DSNguoiDung()
        {
            // Lấy danh sách các nhà tuyển dụng từ nguồn dữ liệu (ví dụ: database)
            var danhSachNhaTuyenDung = await _context.Users.ToListAsync();

            // Truyền danh sách nhà tuyển dụng tới view
            return View(danhSachNhaTuyenDung);
        }
        [HttpGet]
        public IActionResult AddVTCongViec()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddVTCongViec(ViTriCongViec viTriCongViec)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(viTriCongViec.ViTriCongViec_name))
                {
                    TempData["ErrorMessage"] = "Vui lòng nhập thông tin đầy đủ";
                    return View(viTriCongViec);
                }

                await _viTriCongViec.AddAsync(viTriCongViec);
                TempData["SuccessMessage"] = "Đã thêm vị trí công việc thành công";
                return RedirectToAction("ListVTCongViec");
            }
            return View(viTriCongViec);
        }

        public async Task<IActionResult> ListVTCongViec()
        {
            var viTriCongViecs = await _viTriCongViec.GetAllAsync();
            return View(viTriCongViecs);
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
                return RedirectToAction("ListThanhPho", "AD");
            }
            return View(thanhPho);
        }
        public async Task<IActionResult> ListThanhPho()
        {
            var thanhPho = await _thanhPho.GetAllAsync();
            var sortedThanhPho = thanhPho.OrderBy(tp => tp.ThanhPho_name).ToList();
            return View(sortedThanhPho);
        }
        public async Task<IActionResult> DetailsBaiTuyenDung(int id)
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
                return RedirectToAction("ListChuyenNganh", "AD");
            }
            return View(chuyenNganh);
        }
        public async Task<IActionResult> ListChuyenNganh()
        {
            var chuyenNganh = await _chuyenNganh.GetAllAsync();

            return View(chuyenNganh);
        }

        public async Task<IActionResult> DSUngVien()
        {
            var baiTuyenDung = await _baiTuyenDung.GetAllAsync();
            var user = await _userManager.GetUserAsync(User);
            var ungTuyen = await _ungTuyen.GetAllAsync();
            var danhSachUngVien = await _userRepository.GetAllUserAsync();
            // Truyền danh sách nhà tuyển dụng tới view
            return View(danhSachUngVien);
        }
        public async Task<IActionResult> DSNhaTuyenDung()
        {
            var danhSachNhaTuyenDung = await _userRepository.GetAllCompanyAsync();
            ViewBag.NhaTuyenDung = danhSachNhaTuyenDung;
            return View(danhSachNhaTuyenDung);
        }

        public async Task<IActionResult> IndexProfileNTD(string id)
        {
            var find_ungvien = await _userManager.FindByIdAsync(id);
            return View(find_ungvien);
        }


        public async Task<IActionResult> IndexProfileUV(string id)
        {

            var find_ungvien = await _userManager.FindByIdAsync(id);
            return View(find_ungvien);

        }
        public async Task<IActionResult> ListBaiTuyenDung()
        {
            var baiTuyenDungs = await _context.baiTuyenDungs
    .Where(b => b.TrangThai == true)
    .Include(b => b.applicationUser)  // Tải thông tin người dùng
    .ToListAsync();
            return View(baiTuyenDungs);
        }
        public async Task<IActionResult> ThongKe()
        {
            var sotaikhoan = await _userRepository.CountUsersAsync();
            var soNhaTuyenDung = await _userRepository.CountUsersInRoleNTDAsync();
            var soUngVien = await _userRepository.CountUsersInRoleUVAsync();
            var soBaiTuyenDUng = await _baiTuyenDung.CountBaiTuyenDungAsync();
            var soUngTuyen = await _ungTuyen.CountUngTuyenAsync();
            var sonewBTD = await _baiTuyenDung.CountBaiTuyenDungTodayAsync();
            var sonewUT = await _ungTuyen.CountUngTuyenTodayAsync();
            var soUVnew = await _userRepository.CountNewUVTodayAsync();
            var sonewNTD = await _userRepository.CountUsersInRoleNTDAsync();
            var thongKeViewModel = new ThongKe
            {
                slTaiKhoan = sotaikhoan,
                slbaiutinday = sonewUT,
                slbtdinday = sonewBTD,
                slntdinday= sonewNTD,
                sluvinday = soUVnew,
                slNTD = soNhaTuyenDung,
                slUV = soUngVien,
                slBTD = soBaiTuyenDUng,
                slUT = soUngTuyen
            };

            return View(thongKeViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> TimKiem(ViewModel viewModel)
        {
            // Lấy danh sách thành phố và chuyên ngành để hiển thị trên form
            var thanhPho = await _context.thanhPhos.ToListAsync();
            var chuyenNganh = await _context.chuyenNganhs.ToListAsync();
            viewModel.ThanhPhos = thanhPho;
            viewModel.ChuyenNganhs = chuyenNganh;

            // Tạo query tìm kiếm bài tuyển dụng
            var query = _context.baiTuyenDungs.AsQueryable();

            // Tìm kiếm theo tên công việc
            if (!string.IsNullOrEmpty(viewModel.JobName))
            {
                query = query.Where(b => b.TenCongViec.Contains(viewModel.JobName));
            }

            // Nếu người dùng đã chọn thành phố
            if (viewModel.ThanhPhoId != null)
            {
                // Lọc kết quả theo thành phố
                query = query.Where(b => b.thanhPhoId == viewModel.ThanhPhoId);

            }

            /*            // Nếu người dùng đã chọn chuyên ngành
                        if (viewModel.chuyenNganhId != null)
                        {
                            // Lọc kết quả theo chuyên ngành
                            query = query.Where(b => b.ChuyenNganhIds == viewModel.chuyenNganhId);
                        }*/

            // Gán danh sách bài tuyển dụng vào view model
            viewModel.BaiTuyenDungs = await query.ToListAsync();

            return View(viewModel);
        }
    }
}
