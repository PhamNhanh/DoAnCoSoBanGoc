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
        private readonly IUserRepository _userRepository;
        private readonly IKyNangMem _kyNangMem;
        public HomeController(ApplicationDbContext context,
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
            {
                var baiTuyenDung = await _baiTuyenDung.GetAllAsync();
                return View(baiTuyenDung);
            }
        }
        public async Task<IActionResult> IndexAll()
        {

            var baiTuyenDungs = await _context.baiTuyenDungs.ToListAsync();
            var thanhPho = await _context.thanhPhos.ToListAsync();
            var chuyenNganh = await _context.chuyenNganhs.ToListAsync();
            var viewModel = new ViewModel
            {
                BaiTuyenDungs = baiTuyenDungs,
                ThanhPhos = thanhPho,
                ChuyenNganhs = chuyenNganh,
            };
            // Truyền danh sách bài tuyển dụng tới view
            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> TimKiem(ViewModel viewModel)
        {
            // Lấy danh sách thành phố và chuyên ngành để hiển thị trên form
            var thanhPho = await _context.thanhPhos.ToListAsync();
            var chuyenNganh = await _context.chuyenNganhs.ToListAsync();
            viewModel.ThanhPhos = thanhPho;
            viewModel.ChuyenNganhs = chuyenNganh;

            // Nếu người dùng đã chọn thành phố
            if (viewModel.ThanhPhoId != null)
            {
                // Tìm kiếm bài tuyển dụng dựa trên thành phố
                var query = _context.baiTuyenDungs.Where(b => b.thanhPhoId == viewModel.ThanhPhoId);

                // Nếu người dùng đã chọn chuyên ngành
                if (viewModel.chuyenNganhId != null)
                {
                    // Lọc kết quả theo chuyên ngành
                    query = query.Where(b => b.baiTuyenDung_ChuyenNganhs.Id == viewModel.chuyenNganhId);
                }

                // Gán danh sách bài tuyển dụng vào view model
                viewModel.BaiTuyenDungs = await query.ToListAsync();
            }
            else
            {
                // Nếu không có thành phố được chọn, hiển thị tất cả các bài tuyển dụng
                viewModel.BaiTuyenDungs = await _context.baiTuyenDungs.ToListAsync();
            }

            return View(viewModel);
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
        [HttpGet]
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

            var kyNangMem = await _kyNangMem.GetAllAsync();
            var sortedKyNangMem = kyNangMem.OrderBy(tp => tp.KNMem_name).ToList();

            ViewBag.KinhNghiem = new SelectList(sortedKinhNghiem, "KinhNghiem_id", "NamKinhNghiem");
            ViewBag.ChuyenNganh = new SelectList(sortedChuyenNganh, "ChuyenNganh_id", "ChuyenNganh_name");
            ViewBag.ThanhPho = new SelectList(sortedThanhPho, "ThanhPho_id", "ThanhPho_name");
            ViewBag.ViTriCongViec = new SelectList(sortedVitricongviec, "ViTriCongViec_id", "ViTriCongViec_name");
            ViewBag.KyNangMem = new SelectList(sortedKyNangMem, "KyNangMem_id", "KNMem_name");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddBaiTuyenDung(BaiTuyenDung baiTuyenDung)
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

        public async Task<IActionResult> ListBaiTuyenDung()
        {
            var baiTuyenDung = await _baiTuyenDung.GetAllAsync();
            return View(baiTuyenDung);
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
                /*return NotFound();*/
                return View("Thiếu Id user");
            }
            var post = await _baiTuyenDung.GetByIdAsync(id);
            if (post == null)
            {
                /*return NotFound();*/
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
                ungTuyen.BaiTuyenDungid = post.BaiTuyenDung_id;
                ungTuyen.applicationUser = post.applicationUser; // Đã lưu id Nhà Tuyển Dụng
                ungTuyen.application_Userid = find_user.Id; //Chưa lưuu id Ung Viên
                ungTuyen.ThoiGianUngTuyen = DateTime.Now;
/*                ungTuyen.update_at = DateTime.Now;*/
                await _ungTuyen.AddAsync(ungTuyen);
                return RedirectToAction(nameof(DSUngTuyen));
            }
            else
            {
                return BadRequest("Có gì đó không đúng");
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


        //Nha tuyen dung
        public async Task<IActionResult> ListNhaTuyenDung()
        {
            var listnhatuyendung = await _userRepository.GetAllAsync();
            return View(listnhatuyendung);
        }

        /*public async Task<IActionResult> IndexProfileNTD()
        {
            var find_company = await _userManager.GetUserAsync(User);
            if (find_company != null)
            {
                return View(find_company);
            }
            else
            {
                return NotFound();
            }

        }
        public async Task<IActionResult> UpdateProfileNTD()
        {
            var find_company = await _userManager.GetUserAsync(User);
            return View(find_company);
        }

        [HttpPost]
*//*        [ValidateAntiForgeryToken]*//*
        public async Task<IActionResult> UpdateProfileNTD(string id, ApplicationUser company, IFormFile image_url)
        {
            var find_company = await _userManager.GetUserAsync(User);

            if (find_company != null && id != find_company.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (company != null && find_company != null)
                        {
                            if (image_url != null && IsImageFile(image_url) && IsFileSizeValid(image_url))
                            {
                                // Lưu hình ảnh đại diện
                                find_company.image_url = await SaveImage(image_url);
                            }
                           *//* find_company.NhaTuyenDung_name = company.NhaTuyenDung_name;
                            find_company.DiaChi = company.DiaChi;
                            find_company.FullName = company.FullName;
                            find_company.SDTNhaTuyenDung = company.SDTNhaTuyenDung;
                            find_company.Email = company.Email;*//*
                            find_company.ThoiGianCapNhat = DateTime.Now;
                          *//*  find_company.Website = company.Website;
                            find_company.GioiThieuNhaTuyenDung = company.GioiThieuNhaTuyenDung;*//*
                            await _userManager.UpdateAsync(find_company);
                        }
                    }
                }
                catch
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(find_company);
        }
        private bool IsImageFile(IFormFile file)
        {
            // Kiểm tra phần mở rộng của file có phải là ảnh hay không
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(fileExtension);
        }*/
    }
}
