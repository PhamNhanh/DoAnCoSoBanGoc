using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using SQLitePCL;
using System.Security.Claims;
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
        private readonly ISaveJob _saveJobRepository;

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
            IKyNangMem kyNangMem,
            ISaveJob saveJobRepository)
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
            _saveJobRepository = saveJobRepository;
        }
        public async Task<IActionResult> Index()
        {
            // Lấy thông tin người dùng hiện tại
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            // Lấy danh sách thành phố và chuyên ngành, đã sắp xếp theo tên
            var thanhPho = await _thanhPho.GetAllAsync();
            var sortedThanhPho = thanhPho.OrderBy(tp => tp.ThanhPho_name).ToList();

            var chuyenNganh = await _chuyenNganh.GetAllAsync();
            var sortedChuyenNganh = chuyenNganh.OrderBy(cn => cn.ChuyenNganh_name).ToList();

            // Lấy danh sách chuyên ngành của ứng viên hiện tại
            var userChuyenNganhs = await _context.hocVans
                .Where(uc => uc.applicationUserId == currentUser.Id)
                .Select(uc => uc.chuyenNganhId)
                .ToListAsync();

            // Lấy danh sách bài tuyển dụng gợi ý theo chuyên ngành
            var suggestedJobs = new List<BaiTuyenDung>();
            foreach (var chuyenNganhId in userChuyenNganhs)
            {
                var baiTuyenDungsByChuyenNganh = await _baiTuyenDung.GetBaiTuyenDungByChuyenNganhAsync(chuyenNganhId);
                suggestedJobs.AddRange(baiTuyenDungsByChuyenNganh);
            }

            // Lọc các bài tuyển dụng theo trạng thái và loại bỏ các bản sao
            var baiTuyenDungs = suggestedJobs
                .Where(b => b.TrangThai == true)
                .Distinct()
                .ToList();

            // Lấy danh sách ứng viên
            var ungvien = await _userRepository.GetAllAsync();

            // Đếm số lượng bài tuyển dụng theo chuyên ngành
            var jobCountsByMajor = new List<MajorViewModel>();
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

            // Thêm danh sách bài tuyển dụng vào ViewBag
            ViewBag.BaiTuyenDung = baiTuyenDungs;

            // Tạo ViewModel để truyền dữ liệu sang View
            var viewModel = new ViewModel
            {
                BaiTuyenDungs = baiTuyenDungs,
                ThanhPhos = sortedThanhPho,
                ChuyenNganhs = sortedChuyenNganh,
                ApplicationUsers = ungvien,
                Majors = jobCountsByMajor,
                BaiTuyenDung = suggestedJobs // Thêm danh sách gợi ý bài tuyển dụng vào ViewModel
            };

            // Truyền ViewModel sang View
            return View(viewModel);
        }


        public async Task<IActionResult> IndexHocVan(string id)
        {
            var find_user = await _userManager.FindByIdAsync(id);
            var truongdaihoc = await _truongDaiHoc.GetAllAsync();
            var chuyennganh = await _chuyenNganh.GetAllAsync();
            if (find_user != null)
            {
                // Lấy danh sách học vấn của người dùng dựa trên ID của họ
                var hocVanList = await _hocVan.GetByIdUserAsync(find_user.Id);
                return View(hocVanList);

            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet]
        public async Task<IActionResult> AddEducation()
        {
            var chuyenNganh = await _chuyenNganh.GetAllAsync();
            var truongDaiHoc = await _truongDaiHoc.GetAllAsync();
            ViewBag.ChuyenNganh = new SelectList(chuyenNganh, "ChuyenNganh_id", "ChuyenNganh_name");
            ViewBag.TruongDaiHoc = new SelectList(truongDaiHoc, "TruongDaiHoc_id", "TruongDaiHoc_name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEducation(HocVan hocVan)
        {
            var find_user = await _userManager.GetUserAsync(User);
            if (find_user == null)
            {
                return NotFound("Chưa đăng nhập");
            }
            if (ModelState.IsValid)
            {
                try
                {

                    var chuyenNganh = await _chuyenNganh.GetAllAsync();
                    var truongDaiHoc = await _truongDaiHoc.GetAllAsync();
                    ViewBag.ChuyenNganh = new SelectList(chuyenNganh, "ChuyenNganh_id", "ChuyenNganh_name");
                    ViewBag.TruongDaiHoc = new SelectList(truongDaiHoc, "TruongDaiHoc_id", "TruongDaiHoc_name");
                    hocVan.applicationUserId = find_user.Id;
                    await _hocVan.AddAsync(hocVan);
                }
                catch
                {
                    return NotFound();
                }
                return RedirectToAction("IndexHocVan", "UV", new { id = find_user.Id });
            }
            return View(find_user);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateEducation(int id)
        {
            var education = await _hocVan.GetByIdAsync(id);
            var major = await _chuyenNganh.GetAllAsync();
            var university = await _truongDaiHoc.GetAllAsync();
            ViewBag.Majors = new SelectList(major, "ChuyenNganh_id", "ChuyenNganh_name");
            ViewBag.Universities = new SelectList(university, "TruongDaiHoc_id", "TruongDaiHoc_name");

            return View(education);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEducation(int id, [Bind("applicationUserId, GPA, NgayBatDau, NgayTotNghiep, TruongDaiHocid")] HocVan hocVan)
        {
            var find_user = await _userManager.GetUserAsync(User);
            if (find_user == null)
            {//Khó xảy ra vì đã chuyển hướng từ phân quyền
                return NotFound("Chưa đăng nhập");
            }
            var find_education = await _hocVan.GetByIdAsync(id);
            if (find_education == null || hocVan == null)
            {
                return NotFound("Id education không hợp lệ");
            }
                if (find_user != null)
                {
                    hocVan.applicationUserId = find_user.Id;
                    find_education.GPA = hocVan.GPA;
                    find_education.NgayBatDau = hocVan.NgayBatDau;
                    find_education.NgayTotNghiep = hocVan.NgayTotNghiep;
                    find_education.TruongDaiHocid = hocVan.TruongDaiHocid;
                    var majors = await _chuyenNganh.GetAllAsync();
                    var universities = await _truongDaiHoc.GetAllAsync();
                    ViewBag.Majors = new SelectList(majors, "ChuyenNganh_id", "ChuyenNganh_name");
                    ViewBag.Universities = new SelectList(universities, "TruongDaiHoc_id", "TruongDaiHoc_name");

                    await _hocVan.UpdateAsync(find_education);
                return RedirectToAction("IndexHocVan", new { id = find_user.Id });
            }
            return View(find_education);
        }
        [HttpGet]
        public async Task<IActionResult> TimKiem(ViewModel viewModel)
        {
            // Lấy danh sách thành phố và chuyên ngành từ cơ sở dữ liệu
            var thanhPho = await _context.thanhPhos.OrderBy(tp => tp.ThanhPho_name).ToListAsync();
            var chuyenNganh = await _context.chuyenNganhs.OrderBy(cn => cn.ChuyenNganh_name).ToListAsync();

            // Gán danh sách đã sắp xếp vào ViewModel
            viewModel.ThanhPhos = thanhPho;
            viewModel.ChuyenNganhs = chuyenNganh;

            // Tạo query tìm kiếm bài tuyển dụng
            var query = _context.baiTuyenDungs
                .Include(b => b.thanhPho) // Include related ThanhPho
                .Include(b => b.baiTuyenDung_ChuyenNganhs) // Include the join table
                .ThenInclude(bcn => bcn.chuyenNganh) // Include ChuyenNganh through the join table
                .AsQueryable();

            // Lọc kết quả theo trạng thái
            query = query.Where(b => b.TrangThai == true);

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

            viewModel.BaiTuyenDungs = await query.ToListAsync();

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
            // Get the current user's iduser
            var currentUser = await _userManager.GetUserAsync(User);
            var currentUserId = currentUser?.Id; // Assuming Id is the property representing iduser

            // Lấy danh sách ứng tuyển dựa trên IdBaiTuyenDung và iduser của người đăng nhập
            var ungTuyenList = await _ungTuyen.GetUngTuyenByBaiTuyenDungIdAndUserIdAsync(id, currentUserId);


            return View(ungTuyenList);
    }







        public async Task<IActionResult> DeleteEducation(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var education = await _hocVan.GetByIdAsync(id);
            if (education == null)
            {
                return NotFound();
            }
            await _hocVan.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
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

            // Lọc những bài tuyển dụng có TrangThai=true
            var baiTuyenDungTrangThaiTrue = baiTuyenDung.Where(bai => bai.TrangThai == true).ToList();

            ViewBag.NhaTuyenDung = danhSachNhaTuyenDung;

            return View(baiTuyenDungTrangThaiTrue);
        }

        [HttpGet]
        public async Task<IActionResult> UngTuyen(int Id)
        {
            var baiTuyenDung = await _baiTuyenDung.GetByIdAsync(Id);
            var nguoiDung = await _userManager.GetUserAsync(User);
            if (baiTuyenDung == null || nguoiDung == null)
            {
                return NotFound(); 
            }
            ViewBag.NguoiDung = nguoiDung;
            ViewBag.BaiTuyenDung = baiTuyenDung;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UngTuyen(UngTuyen ungTuyen, IFormFile url_cv, int id/*, string TenUngVien, string EmailUV*/)
        {
            // id người dùng
            var find_user = await _userManager.GetUserAsync(User);
            // id bài tuyển dụng
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
/*                ungTuyen.TenUngVien = TenUngVien;
                ungTuyen.EmailUV = EmailUV;*/
                ungTuyen.BaiTuyenDungid = post.BaiTuyenDung_id;
                ungTuyen.applicationUser = post.applicationUser; // Đã lưu id Nhà Tuyển Dụng
                ungTuyen.application_Userid = find_user.Id; // Đã lưu id Ứng Viên
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

            if (find_company == null || id != find_company.Id)
            {
                return NotFound();
            }

/*            if (ModelState.IsValid)
            {*/
                try
                {
                    if (image_url != null && IsImageFile(image_url) && IsFileSizeValid(image_url))
                    {
                        // Lưu hình ảnh đại diện
                        find_company.image_url = await SaveImage(image_url);
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
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ
                    // Ghi log lỗi hoặc hiển thị thông báo lỗi
                    return BadRequest("Đã xảy ra lỗi khi cập nhật thông tin.");
                }
        

/*            // ModelState không hợp lệ, hiển thị lại form với thông báo lỗi
            return View(find_company);*/
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
        /*        public async Task<IActionResult> ViewSavedJobs()
                {
                    // Get the current logged-in user's ID
                    var userId = _userManager.GetUserId(User);

                    // Retrieve the saved jobs for this user
                    var savedJobs = await _saveJobRepository.GetSavedJobsAsync(userId);

                    // Prepare the model to be passed to the view
                    var model = new ListSave
                    {
                        SavedJobs = savedJobs
                    };

                    // Return the view with the model
                    return View(model);
                }*/

        [HttpPost]
        public async Task<IActionResult> SaveJob(int jobId)
        {
            var userId = _userManager.GetUserId(User);  // Lấy UserId từ người dùng hiện tại

            // Gọi phương thức SaveJobAsync từ repository với userId và jobId
            await _saveJobRepository.SaveJobAsync(userId, jobId);

            // Chuyển hướng về trang danh sách công việc đã lưu
            return RedirectToAction("ViewSavedJobs");
        }
        public IActionResult LoadMoreJobs(int page)
        {
            // Lấy danh sách bài tuyển dụng theo page
            var jobs = _context.baiTuyenDungs
                .Skip(page * 6) // Bỏ qua các bài tuyển dụng đã hiển thị
                .Take(6) // Lấy thêm 6 bài tuyển dụng
                .ToList();

            return PartialView("_JobList", jobs); // Trả về partial view chứa danh sách bài tuyển dụng
        }
        // Action để xem chi tiết một bài tuyển dụng
        public async Task<IActionResult> ListUngTuyen( string id)
        {
            var applications = await _ungTuyen.GetAllAsync(); // Lấy tất cả ứng tuyển từ cơ sở dữ liệu

            if (applications == null || !applications.Any())
            {
                ViewBag.Message = "Hiện tại không có bài ứng tuyển nào.";
            }

            // Trả về view với tất cả các ứng tuyển
            return View(applications);
        }


        [HttpPost]
        public async Task<IActionResult> ListUngTuyen(IFormCollection form)
        {
            var ungTuyenList = await _ungTuyen.GetAllAsync(); // Lấy tất cả ứng tuyển

            if (ungTuyenList == null || !ungTuyenList.Any())
            {
                return RedirectToAction("ListUngTuyen");
            }

            foreach (var ungtuyen in ungTuyenList)
            {
                string formKey = $"status_{ungtuyen.UngTuyen_id}";
                if (form.TryGetValue(formKey, out var statusValue))
                {
                    // Cập nhật trạng thái từ form
                    ungtuyen.TrangThai = statusValue;
                    _context.ungTuyens.Update(ungtuyen);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ListUngTuyen)); // Chuyển hướng lại để hiển thị danh sách đã cập nhật
        }

    }
}
