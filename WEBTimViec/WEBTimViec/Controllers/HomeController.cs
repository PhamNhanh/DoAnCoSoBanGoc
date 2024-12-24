using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WEBTimViec.Data;
using WEBTimViec.Models;
using WEBTimViec.Repositories;
using System.Linq;
using PagedList.Mvc;
using PagedList;
using System.Drawing.Printing;

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
        public async Task<IActionResult> IndexAll(int? pageNumber, int pageSize = 6)
        {
            var baiTuyenDungs = await _context.baiTuyenDungs
                                               .Where(b => b.TrangThai == true) // Chỉ lấy bài tuyển dụng có trạng thái = 1
                                               .Include(b => b.applicationUser)
                                               .OrderByDescending(b => b.ThoiGianDangBai)// Bao gồm thông tin của ApplicationUser
                                               .ToListAsync();

            // Áp dụng phân trang cho danh sách bài tuyển dụng
            int currentPage = pageNumber ?? 1;
            var pagedBaiTuyenDungs = baiTuyenDungs.ToPagedList(currentPage, pageSize);
            var totalCount = _context.baiTuyenDungs.Count();  // Đếm tổng số bài tuyển dụng
            var pageCount = (int)Math.Ceiling((double)totalCount / pageSize);  // Tính số trang
            // Lấy danh sách thành phố và sắp xếp theo tên
            var thanhPho = await _thanhPho.GetAllAsync();
            var sortedThanhPho = thanhPho.OrderBy(tp => tp.ThanhPho_name).ToList();

            // Lấy danh sách chuyên ngành và sắp xếp theo tên
            var chuyenNganh = await _chuyenNganh.GetAllAsync();
            var sortedChuyenNganh = chuyenNganh.OrderBy(cn => cn.ChuyenNganh_name).ToList();

            // Lấy danh sách ứng viên
            var ungvien = await _userRepository.GetAllAsync();

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

            // Sắp xếp các chuyên ngành theo số lượng bài tuyển dụng giảm dần và giới hạn 2 chuyên ngành
            jobCountsByMajor = jobCountsByMajor
                .OrderByDescending(m => m.JobCount)
                .Take(2)
                .ToList();

            // Tạo ViewModel để truyền dữ liệu sang View
            var viewModel = new ViewModel
            {
                BaiTuyenDungs = pagedBaiTuyenDungs, // Sử dụng danh sách đã phân trang
                ThanhPhos = sortedThanhPho,
                ChuyenNganhs = sortedChuyenNganh,
                ApplicationUsers = ungvien,
                Majors = jobCountsByMajor, // Thêm danh sách chuyên ngành được sắp xếp với số lượng bài tuyển dụng
               TotalCount = totalCount,  // Truyền tổng số bài tuyển dụng vào ViewModel
                PageCount = pageCount
            };

            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> TimKiem(string JobName, int? ThanhPhoId)
        {
            // Khởi tạo ViewModel
            var viewModel = new ViewModel
            {
                ThanhPhos = await _context.thanhPhos.ToListAsync(),
                ChuyenNganhs = await _context.chuyenNganhs.ToListAsync()
            };

            // Tạo query tìm kiếm bài tuyển dụng
            var query = _context.baiTuyenDungs
                .Where(b => b.TrangThai == true) // Chỉ lấy bài tuyển dụng có trạng thái = 1
                .Include(b => b.applicationUser) // Bao gồm thông tin ApplicationUser
                .AsQueryable();

            // Lọc theo tên công việc
            if (!string.IsNullOrEmpty(JobName))
            {
                query = query.Where(b => b.TenCongViec.Contains(JobName));
            }

            // Lọc theo thành phố (nếu có)
            if (ThanhPhoId.HasValue)
            {
                query = query.Where(b => b.thanhPhoId == ThanhPhoId.Value);
            }

            // Thực thi query và truyền dữ liệu vào ViewModel
            viewModel.BaiTuyenDungs = await query
                .OrderByDescending(b => b.ThoiGianDangBai) // Sắp xếp theo thời gian đăng bài
                .ToListAsync();

            return View(viewModel);
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
        public async Task<IActionResult> DetailsBaiTuyenDung(int id)
        {
            // Lấy bài tuyển dụng kèm theo các thông tin liên quan
            var baiTuyenDung = await _context.baiTuyenDungs
                .Include(b => b.baiTuyenDung_ChuyenNganhs)
                    .ThenInclude(bcn => bcn.chuyenNganh)
                .Include(b => b.baiTuyenDung_ViTris)
                    .ThenInclude(bv => bv.viTriCongViec)
                .Include(b => b.baiTuyenDung_KyNangMems)
                    .ThenInclude(bk => bk.kyNangMem)
                .Include(b => b.applicationUser) // Bao gồm thông tin công ty (nếu liên kết với ApplicationUser)
                .FirstOrDefaultAsync(b => b.BaiTuyenDung_id == id);

            if (baiTuyenDung == null)
            {
                return NotFound();
            }

            // Lấy tên thành phố từ bảng ThanhPho dựa trên thanhPhoId
            var thanhPho = await _context.thanhPhos
                .Where(tp => tp.ThanhPho_id == baiTuyenDung.thanhPhoId)
                .FirstOrDefaultAsync();

            if (thanhPho != null)
            {
                // Truyền thông tin thành phố vào ViewBag
                ViewBag.TenTP = thanhPho.ThanhPho_name;
                ViewBag.KinhDo = thanhPho.kinhDo;
                ViewBag.ViDo = thanhPho.viDo;
            }
            else
            {
                ViewBag.TenTP = "Thông tin thành phố không có sẵn"; // Trường hợp không tìm thấy thành phố
            }

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

            // Lấy thông tin công ty
            if (baiTuyenDung.applicationUser != null)
            {
                ViewBag.TenCongTy = baiTuyenDung.applicationUser.NhaTuyenDung_name; // Giả sử applicationUser có thuộc tính CompanyName
            }
            else
            {
                ViewBag.TenCongTy = "Thông tin công ty không có sẵn";
            }

            return View(baiTuyenDung);
        }

        public async Task<IActionResult> ListNhaTuyenDung()
        {
            var listnhatuyendung = await _userRepository.GetAllAsync();
            return View(listnhatuyendung);
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

    }
}
