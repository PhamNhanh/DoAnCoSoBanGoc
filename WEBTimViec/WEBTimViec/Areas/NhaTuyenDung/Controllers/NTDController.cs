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
    public class NTDController : Controller
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

        public NTDController(ApplicationDbContext context,
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
        public async Task<IActionResult> ListBaiTuyenDung()
        {
            var baiTuyenDung = await _baiTuyenDung.GetAllAsync();
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
                    return View(baiTuyenDung);
                }

                if (baiTuyenDung.Luong_min >= baiTuyenDung.Luong_max)
                {
                    ModelState.AddModelError(nameof(baiTuyenDung.Luong_max), "Lương từ phải nhỏ hơn lương đến.");
                    return View(baiTuyenDung);
                }

                _context.baiTuyenDungs.Add(baiTuyenDung);
                await _context.SaveChangesAsync();

                if (baiTuyenDung.ChuyenNganhIds != null)
                {
                    foreach (var chuyenNganhId in baiTuyenDung.ChuyenNganhIds)
                    {
                        var baiTuyenDungChuyenNganh = new BaiTuyenDung_ChuyenNganh
                        {
                            BaiTuyenDungid = baiTuyenDung.BaiTuyenDung_id,
                            ChuyenNganhid = chuyenNganhId
                        };
                        _context.baiTuyenDung_ChuyenNganhs.Add(baiTuyenDungChuyenNganh);
                    }
                    await _context.SaveChangesAsync();
                }
                if (baiTuyenDung.KyNangMemIds != null)
                {
                    foreach (var kyNangMemId in baiTuyenDung.KyNangMemIds)
                    {
                        var baiTuyenDungKyNangMem = new BaiTuyenDung_KyNangMem
                        {
                            BaiTuyenDungid = baiTuyenDung.BaiTuyenDung_id,
                            KyNangMemid = kyNangMemId
                        };
                        _context.baiTuyenDung_KyNangMems.Add(baiTuyenDungKyNangMem);
                    }
                    await _context.SaveChangesAsync();
                }
                if (baiTuyenDung.ViTriCongViecIds != null)
                {
                    foreach (var viTriCongViec in baiTuyenDung.ViTriCongViecIds)
                    {
                        var baiTuyenDungVITri = new BaiTuyenDung_ViTri
                        {
                            BaiTuyenDungid = baiTuyenDung.BaiTuyenDung_id,
                            ViTriCongViecid = viTriCongViec
                        };
                        _context.baiTuyenDung_ViTris.Add(baiTuyenDungVITri);
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index", "NTD");
            }
            return View(baiTuyenDung);
        }
        public async Task<IActionResult> IndexProfileNTD()
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
                    find_company.NhaTuyenDung_name = company.NhaTuyenDung_name;
                    find_company.SDTNhaTuyenDung = company.SDTNhaTuyenDung;
                    find_company.Website = company.Website;
                    find_company.FullName = company.FullName;
                    find_company.DiaChi = company.DiaChi;
                    find_company.GioiThieuNhaTuyenDung = company.GioiThieuNhaTuyenDung;
                    find_company.ThoiGianCapNhat = DateTime.Now;

                    // Lưu thay đổi vào cơ sở dữ liệu
                    await _userManager.UpdateAsync(find_company);

                    return RedirectToAction(nameof(IndexProfileNTD));
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
