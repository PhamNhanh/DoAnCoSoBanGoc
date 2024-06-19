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
        private readonly IHocVan _hocVan;
        private readonly ITruongDaiHoc _truongDaiHoc;
        public NTDController(ApplicationDbContext context,
            IBaiTuyenDung baiTuyenDung,
            IChuyenNganh chuyenNganh,
            IThanhPho thanhPho,
            IKinhNghiem kinhNghiem,
            UserManager<ApplicationUser> userManager,
            IUngTuyen ungTuyen,
            IUserRepository userRepository,
            IViTriCongViec viTriCongViec,
            ITruongDaiHoc truongDaiHoc,
            IKyNangMem kyNangMem,
            IHocVan hocVan)
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
            // Lấy danh sách bài tuyển dụng có trạng thái là true
            var baiTuyenDungs = await _context.baiTuyenDungs
                                        .Where(b => b.TrangThai == true) // Chỉ lấy bài tuyển dụng có trạng thái là true
                                        .ToListAsync();

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

            // Sắp xếp các chuyên ngành theo số lượng bài tuyển dụng giảm dần
            jobCountsByMajor = jobCountsByMajor.OrderByDescending(m => m.JobCount).ToList();

            // Tạo ViewModel để truyền dữ liệu sang View
            var viewModel = new ViewModel
            {
                BaiTuyenDungs = baiTuyenDungs,
                ThanhPhos = sortedThanhPho,
                ChuyenNganhs = sortedChuyenNganh,
                ApplicationUsers = ungvien,
                Majors = jobCountsByMajor // Thêm danh sách chuyên ngành được sắp xếp với số lượng bài tuyển dụng
            };

            // Thêm thông tin applicationUser cho mỗi bài tuyển dụng
            foreach (var baiTuyenDung in viewModel.BaiTuyenDungs)
            {
                var applicationUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == baiTuyenDung.ApplicationUserId);
                baiTuyenDung.applicationUser = applicationUser;
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> TimKiem(ViewModel viewModel)
        {
            // Lấy danh sách thành phố và chuyên ngành từ cơ sở dữ liệu và sắp xếp theo tên
            var thanhPho = await _context.thanhPhos.OrderBy(tp => tp.ThanhPho_name).ToListAsync();
            var chuyenNganh = await _context.chuyenNganhs.OrderBy(cn => cn.ChuyenNganh_name).ToListAsync();

            // Gán danh sách đã sắp xếp vào ViewModel
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

            viewModel.BaiTuyenDungs = await query.ToListAsync();

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

            ViewBag.ChuyenNganhs = baiTuyenDung.baiTuyenDung_ChuyenNganhs
                .Where(bcn => bcn.BaiTuyenDungid == id)
                .Select(bcn => bcn.chuyenNganh.ChuyenNganh_name)
                .ToList();

            ViewBag.ViTriCongViecs = baiTuyenDung.baiTuyenDung_ViTris
                .Where(bv => bv.BaiTuyenDungid == id)
                .Select(bv => bv.viTriCongViec.ViTriCongViec_name)
                .ToList();

            ViewBag.KyNangMems = baiTuyenDung.baiTuyenDung_KyNangMems
                .Where(bk => bk.BaiTuyenDungid == id)
                .Select(bk => bk.kyNangMem.KNMem_name)
                .ToList();

            return View(baiTuyenDung);
        }
        public async Task<IActionResult> ListBaiTuyenDung(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var DSBaiTuyenDung = await _baiTuyenDung.GetBaiTuyenDungByUserIdAsync(user.Id);
            var baiTuyenDungTrangThaiTrue = DSBaiTuyenDung.Where(bai => bai.TrangThai == true).ToList();

            return View(baiTuyenDungTrangThaiTrue);
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

        public async Task<IActionResult> DSUngTuyen(string id)
        {
            var find_user = await _userManager.GetUserAsync(User);
            if (find_user != null)
            {
                // Lấy danh sách các bài tuyển dụng có trạng thái là true
                var baituyendung = await _baiTuyenDung.GetBaiTuyenDungByUserIdAsync(find_user.Id);
                var filteredBaiTuyenDung = baituyendung.Where(bt => bt.TrangThai == true);
                return View(filteredBaiTuyenDung);
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> DetailsUngTuyen(int id)
        {
            var ungTuyenList = await _ungTuyen.GetUngTuyenByBaiTuyenDungIdAsync(id);

            if (ungTuyenList == null || !ungTuyenList.Any())
            {
                ViewData["Message"] = "Hiện tại không có ứng viên nào cho bài tuyển dụng này.";
                return View(); // Trả về view mà không có dữ liệu
            }

            return View(ungTuyenList);
        }

        [HttpPost]
        public async Task<IActionResult> SaveFeedback(int UngTuyen_id, int BaiTuyenDung_id, string TrangThai)
        {
            var ungTuyen = await _ungTuyen.GetByIdAsync(UngTuyen_id);

            if (ungTuyen != null && ungTuyen.BaiTuyenDungid == BaiTuyenDung_id)
            {
                ungTuyen.TrangThai = TrangThai;
                await _ungTuyen.UpdateAsync(ungTuyen);
                return Ok(new { success = true });
            }

            return BadRequest(new { success = false, message = "Không tìm thấy ứng tuyển hoặc thông tin không hợp lệ." });
        }
        [HttpPost]
        public async Task<IActionResult> DetailsUngTuyen(IFormCollection form)
        {
            var ungTuyenList = await _ungTuyen.GetAllAsync();

            if (!ungTuyenList.Any())
            {
                // Trả về view với thông báo nếu không có bài ứng tuyển nào trong danh sách
                ViewBag.Message = "Không có bài ứng tuyển nào.";
                return View();  
            }

            foreach (var ungtuyen in ungTuyenList)
            {
                string formKey = $"status_{ungtuyen.UngTuyen_id}";
                if (form.TryGetValue(formKey, out var statusValue))
                {
                    ungtuyen.TrangThai = statusValue;
                    _context.ungTuyens.Update(ungtuyen);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(DetailsUngTuyen));
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
            try
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
                    baiTuyenDung.TrangThai = true;
                    baiTuyenDung.ThoiGianDangBai = DateTime.Now;
                    if (baiTuyenDung.Luong_min < 0)
                    {
                        ModelState.AddModelError(nameof(baiTuyenDung.Luong_min), "Lương từ không được âm.");
                        return RedirectToAction("AddBaiTuyenDung", "NTD");
                    }

                    if (baiTuyenDung.Luong_min >= baiTuyenDung.Luong_max)
                    {
                        ModelState.AddModelError(nameof(baiTuyenDung.Luong_max), "Lương từ phải nhỏ hơn lương đến.");
                        return RedirectToAction("AddBaiTuyenDung", "NTD");
                    }
                    else
                    {
                        _context.baiTuyenDungs.Add(baiTuyenDung);
                        await _context.SaveChangesAsync();
                    }


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
            catch (Exception ex)
            {
                return RedirectToAction("AddBaiTuyenDung", "NTD");
            }
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
        public async Task<IActionResult> IndexProfileUV(string id)
        {

            var find_ungvien = await _userManager.FindByIdAsync(id);
            return View(find_ungvien);

        }

        [HttpGet]
        public async Task<IActionResult> UpdateProfileNTD()
        {
            var find_company = await _userManager.GetUserAsync(User);
            return View(find_company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfileNTD(string id, [Bind("NhaTuyenDung_name, SDTNhaTuyenDung, Website, FullName, DiaChi,GioiThieuNhaTuyenDung")] ApplicationUser company, IFormFile image_url)
        {
            var find_company = await _userManager.GetUserAsync(User);

            if (find_company != null && id != find_company.Id)
            {
                return NotFound("Khong tim thay 1");
            }
/*
            if (ModelState.IsValid)
            {*/
                try
                {
                    if (image_url != null && IsImageFile(image_url) && IsFileSizeValid(image_url))
                    {
                        // Lưu hình ảnh đại diện
                        find_company.image_url = await SaveImage(image_url);
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
                }
                catch(Exception e)
                {
                    throw new Exception(e.Message);
                }
                return RedirectToAction(nameof(IndexProfileNTD));
/*            }

            return View(find_company);*/
        }

        public async Task<IActionResult> DetailsHocVan(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var find_user = await _context.Users.FindAsync(id);
            if (find_user == null)
            {
                return NotFound();
            }

            var hocVanList = await _hocVan.GetByIdUserAsync(find_user.Id);
            var hocVan = hocVanList.FirstOrDefault(); // Lấy phần tử đầu tiên từ danh sách

            if (hocVan == null)
            {
                return NotFound();
            }
            // Lấy danh sách trường đại học và chuyên ngành
            var truongDaiHoc = await _truongDaiHoc.GetAllAsync();
            var chuyenNganh = await _chuyenNganh.GetAllAsync();
            ViewBag.ChuyenNganh = new SelectList(truongDaiHoc, "ChuyenNganh_id", "ChuyenNganh_name");
            ViewBag.ThanhPho = new SelectList(chuyenNganh, "ThanhPho_id", "ThanhPho_name");
            return View(hocVan);
        }
        public async Task<IActionResult> DetailsProfileUV(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var find_user = await _context.Users.FindAsync(id);
            if (find_user == null)
            {
                return NotFound();
            }

            var hocVan = await _hocVan.GetByIdUserAsync(find_user.Id);
            if (hocVan == null)
            {
                return NotFound();
            }

            ViewBag.HocVan = hocVan;
            return View(find_user);
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
        public async Task<IActionResult> DeleteBTD(int id)
        {
            // Tìm bài tuyển dụng theo ID
            var baiTuyenDung = await _context.baiTuyenDungs.FirstOrDefaultAsync(b => b.BaiTuyenDung_id == id);

            if (baiTuyenDung == null)
            {
                return NotFound();
            }

            // Cập nhật thuộc tính TrangThai thành false (0)
            baiTuyenDung.TrangThai = false;

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.baiTuyenDungs.Update(baiTuyenDung);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var baiTuyenDung = await _context.baiTuyenDungs.FirstOrDefaultAsync(b => b.BaiTuyenDung_id == id);
            if (baiTuyenDung != null)
            {
                // Cập nhật thuộc tính TrangThai thành false (0)
                baiTuyenDung.TrangThai = false;

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.baiTuyenDungs.Update(baiTuyenDung);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ListBaiTuyenDung));
        }


    }
}
