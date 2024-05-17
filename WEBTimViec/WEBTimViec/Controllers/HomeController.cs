using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WEBTimViec.Data;
using WEBTimViec.Models;
using WEBTimViec.Repositories;

namespace WEBTimViec.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBaiTuyenDung _baiTuyenDung;
        private readonly IChuyenNganh _chuyenNganh;
        private readonly IHocVan _hocVan;
/*        private readonly UserManager<ApplicationUser> _userManager;*/
        private readonly ApplicationDbContext _context;
        private readonly IKinhNghiem _kinhNghiem;
        private readonly IUngTuyen _ungTuyen;

        public HomeController(ApplicationDbContext context,
            IBaiTuyenDung baiTuyenDung,
            IChuyenNganh chuyenNganh,
            IHocVan hocVan,
            IKinhNghiem kinhNghiem,
/*            UserManager<ApplicationUser> userManager,*/
            IUngTuyen ungTuyen)
        {
            _context = context;
            _baiTuyenDung = baiTuyenDung;
            _chuyenNganh = chuyenNganh;
            _kinhNghiem = kinhNghiem;
            _hocVan = hocVan;
/*            _userManager = userManager;*/
            _ungTuyen = ungTuyen;
        }
        public IActionResult Index()
        {
            /*            var hienthi_NhaTuyenDung = await _userManager.GetUserAsync(User);*/

            return View();
        }

    }
}
