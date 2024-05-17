using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WEBTimViec.Data;
using WEBTimViec.Models;
using WEBTimViec.Repositories;

namespace portal_job_FN.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class HomeController : Controller
    {
        private readonly IKinhNghiem _kinhNghiem;
        private readonly IChuyenNganh _chuyenNganh;
        private readonly ITruongDaiHoc _truongDaiHoc;
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context,
            IKinhNghiem kinhNghiem,
            IChuyenNganh chuyenNganh,
            ITruongDaiHoc truongDaiHoc,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _chuyenNganh = chuyenNganh;
            _kinhNghiem = kinhNghiem;
            _truongDaiHoc = truongDaiHoc;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var find_admin = await _userManager.GetUserAsync(User);
            if (find_admin == null)
            {
                return NotFound("Chưa đăng nhập");
            }
            return View(find_admin);
        }
    }
}
