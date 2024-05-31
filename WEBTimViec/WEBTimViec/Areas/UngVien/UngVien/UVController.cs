using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
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
        private readonly IHocVan _hocVan;
        private readonly ITruongDaiHoc _truongDaiHoc;
        public UVController(ApplicationDbContext context,
            IBaiTuyenDung baiTuyenDung,
            IChuyenNganh chuyenNganh,
            IThanhPho thanhPho,
            IKinhNghiem kinhNghiem,
            UserManager<ApplicationUser> userManager,
            IUngTuyen ungTuyen,
            IUserRepository userRepository,
            IViTriCongViec viTriCongViec,
            IHocVan hocVan,
            ITruongDaiHoc truongDaiHoc,
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
            _truongDaiHoc = truongDaiHoc;
            _kyNangMem = kyNangMem;
            _hocVan = hocVan;
        }
        public async Task<IActionResult> Index()
        {
            var baiTuyenDungs = await _context.baiTuyenDungs.ToListAsync();
            var thanhPho = await _thanhPho.GetAllAsync();
            var sortedThanhPho = thanhPho.OrderBy(tp => tp.ThanhPho_name).ToList(); var chuyenNganh = await _chuyenNganh.GetAllAsync();
            var sortedChuyenNganh = chuyenNganh.OrderBy(cn => cn.ChuyenNganh_name).ToList();
            var chuyenNganhs = await _context.chuyenNganhs.ToListAsync();
            var viewModel = new ViewModel
            {
                
                BaiTuyenDungs = baiTuyenDungs,
                ThanhPhos = sortedThanhPho,
                ChuyenNganhs = sortedChuyenNganh,
            };
            // Truyền danh sách bài tuyển dụng tới view
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AddHocVan()
        {

            var chuyenNganh = await _chuyenNganh.GetAllAsync();
            var truongDaiHoc = await _truongDaiHoc.GetAllAsync();
            ViewBag.ChuyenNganh = new SelectList(chuyenNganh, "ChuyenNganh_id", "ChuyenNganh_name");
            ViewBag.TruongDaiHoc = new SelectList(truongDaiHoc, "TruongDaiHoc_id", "TruongDaiHoc_name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddHocVan(HocVan hocVan)
        {
            var find_user = await _userManager.GetUserAsync(User);
            if (find_user == null)
            {//Khó xảy ra vì đã chuyển hướng từ phân quyền
                return NotFound("Chưa đăng nhập");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (find_user != null)
                        {
/*                            find_user.Id = hocVan.applicationUserId;*/
                            var chuyenNganh = await _chuyenNganh.GetAllAsync();
                            var truongDaiHoc = await _truongDaiHoc.GetAllAsync();
                            ViewBag.ChuyenNganh = new SelectList(chuyenNganh, "ChuyenNganh_id", "ChuyenNganh_name");
                            ViewBag.TruongDaiHoc = new SelectList(truongDaiHoc, "TruongDaiHoc_id", "TruongDaiHoc_name");
                            await _hocVan.AddAsync(hocVan);
                        }
                        if (hocVan.ChuyenNganhIds != null)
                        {
                            foreach (var chuyenNganhId in hocVan.ChuyenNganhIds)
                            {
                                var hocVan_ChuyenNganh = new HocVan_ChuyenNganh
                                {
                                    HocVanId = hocVan.HocVan_id,
                                    ChuyenNganhid = chuyenNganhId
                                };
                                _context.hocvan_ChuyenNganhs.Add(hocVan_ChuyenNganh);
                            }
                            await _context.SaveChangesAsync();
                        }
                    }

                }
                catch
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(find_user);
        }
        public async Task<IActionResult> IndexHocVan(string id)
        {
            var find_user = await _userManager.FindByIdAsync(id);
            if (find_user != null)
            {
                var truongDaiHoc = await _truongDaiHoc.GetAllAsync();
                var chuyenNganh = await _chuyenNganh.GetAllAsync();
                var hocVanList = await _hocVan.GetByIdUserAsync(find_user.Id);
                var hocVan = hocVanList.FirstOrDefault(); // Lấy phần tử đầu tiên từ danh sách
                return View(hocVan);
            }
            else
            {
                return NotFound();
            }
        }



        [HttpGet]
        public async Task<IActionResult> TimKiem(ViewModel viewModel)
        {
            // Lấy danh sách thành phố và chuyên ngành để hiển thị trên form
            var baiTuyenDungs = await _context.baiTuyenDungs.ToListAsync();
            var thanhPho = await _thanhPho.GetAllAsync();
            var sortedThanhPho = thanhPho.OrderBy(tp => tp.ThanhPho_name).ToList(); var chuyenNganh = await _chuyenNganh.GetAllAsync();
            var sortedChuyenNganh = chuyenNganh.OrderBy(cn => cn.ChuyenNganh_name).ToList();
            var chuyenNganhs = await _context.chuyenNganhs.ToListAsync();
            viewModel.ThanhPhos = sortedThanhPho;
            viewModel.ChuyenNganhs = sortedChuyenNganh;

            // Nếu người dùng đã chọn thành phố
            if (viewModel.ThanhPhoId != null)
            {
                // Tìm kiếm bài tuyển dụng dựa trên thành phố
                var query = _context.baiTuyenDungs.Where(b => b.thanhPhoId == viewModel.ThanhPhoId);

                // Nếu người dùng đã chọn chuyên ngành
                if (viewModel.chuyenNganhId != null)
                {
                    // Lọc kết quả theo chuyên ngành
                    query = query.Where(b => b.chuyenNganh.ChuyenNganh_id == viewModel.chuyenNganhId);
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
        public async Task<IActionResult> DSNhaTuyenDung()
        {
            var danhSachNhaTuyenDung = await _userRepository.GetAllCompanyAsync();
            ViewBag.NhaTuyenDung = danhSachNhaTuyenDung;
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

        public async Task<IActionResult> DSUngTuyen()
        {
            // Lấy danh sách các bài ứng tuyển từ dịch vụ hoặc cơ sở dữ liệu của bạn
            var danhSachBaiUngTuyen = await _baiTuyenDung.GetAllAsync();

            // Trả về view với danh sách bài ứng tuyển
            return View(danhSachBaiUngTuyen);
        }


        public async Task<IActionResult> DetailsUngTuyen(int id)
        {
            // Lấy danh sách ứng tuyển dựa trên IdBaiTuyenDung
            var ungTuyenList = await _ungTuyen.GetUngTuyenByBaiTuyenDungIdAsync(id);

            if (ungTuyenList == null || !ungTuyenList.Any())
            {
                return NotFound();
            }

            return View(ungTuyenList);
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
            var danhSachNhaTuyenDung = await _userRepository.GetAllCompanyAsync();
            ViewBag.NhaTuyenDung = danhSachNhaTuyenDung;
            return View(baiTuyenDung);
        }
        [HttpGet]
        public async Task<IActionResult> UngTuyen(int Id)
        {
            var baiTuyenDung = await _baiTuyenDung.GetByIdAsync(Id);
            var nguoiDung = await _userManager.GetUserAsync(User);
            ViewBag.NguoiDung = nguoiDung;
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
        public async Task<IActionResult> IndexProfileUV()
        {
            var find_user = await _userManager.GetUserAsync(User);
            if (find_user != null)
            {
                var hocVan = await _hocVan.GetByIdUserAsync(find_user.Id);
                ViewBag.HocVan = hocVan;
                return View(find_user);
            }
            else
            {
                return NotFound();
            }
        }
        public async Task<IActionResult> UpdateProfileUV()
        {
            var find_company = await _userManager.GetUserAsync(User);
            return View(find_company);
        }

        [HttpPost]

        public async Task<IActionResult> UpdateProfileUV(string id, ApplicationUser ungvien, IFormFile image_url)
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
                    if (image_url != null && IsImageFile(image_url) && IsFileSizeValid(image_url))
                    {
                        // Lưu hình ảnh đại diện
                        find_company.image_url = await SaveImage(image_url);
                    }
                    else if (image_url == null)
                    {
                        if (find_company.image_url != null)
                        {
                            // Nếu trong cơ sở dữ liệu có URL hình ảnh được lưu trữ, giữ nguyên giá trị của image_url
                            find_company.image_url = find_company.image_url;
                        }
                        else
                        {
                            // Nếu không có URL hình ảnh trong cơ sở dữ liệu, gán image_url = null
                            find_company.image_url = null;
                        }
                    }


                    // Cập nhật các thông tin khác của công ty
                    find_company.FullName = ungvien.FullName;
                    find_company.SDT_UngVien = ungvien.SDT_UngVien;
                    find_company.NgaySinh = ungvien.NgaySinh;
                    find_company.DiaChi = ungvien.DiaChi;
                    find_company.TuGioiThieu = ungvien.TuGioiThieu;
                    find_company.ThoiGianCapNhat = DateTime.Now;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    await _userManager.UpdateAsync(find_company);

                    return RedirectToAction(nameof(IndexProfileUV));
                }
                catch
                {
                    return NotFound();
                }
            }
            return View(find_company);
        }
        private bool IsImageFile(IFormFile file)
        {
            // Kiểm tra phần mở rộng của file có phải là ảnh hay không
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(fileExtension);
        }
        private bool IsFileSizeValid(IFormFile file)
        {
            // Kiểm tra kích thước file không vượt quá 10MB
            var maxSize = 10 * 1024 * 1024; // 10MB
            return file.Length <= maxSize;
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
