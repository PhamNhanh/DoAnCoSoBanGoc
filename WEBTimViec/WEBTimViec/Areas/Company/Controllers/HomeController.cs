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

namespace portal_job_FN.Areas.Company.Controllers
{
    [Area("Company")]
    [Authorize(Roles = SD.Role_Company)]

    public class HomeController : Controller
    {
        private readonly IKinhNghiem _kinhNghiem;
        private readonly ITruongDaiHoc _truongDaiHoc;
        private readonly IChuyenNganh _chuyenNganh;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context,
            IKinhNghiem kinhNghiem,
            IChuyenNganh chuyenNganh,
            ITruongDaiHoc truongDaiHoc,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _kinhNghiem = kinhNghiem;
            _chuyenNganh = chuyenNganh;
            _truongDaiHoc = truongDaiHoc;
            _userManager = userManager;
        }

        // GET: Company/Home
        public async Task<IActionResult> Index()
        {
            var find_company = await _userManager.GetUserAsync(User);
            if (find_company == null)
            {
                return NotFound("Chưa đăng nhập");
            }
            return View(find_company);
        }
        // GET: Company/Home
        public async Task<IActionResult> _Layout_Company()
        {
            var find_company = await _userManager.GetUserAsync(User);
            if (find_company == null)
            {
                return NotFound("Chưa đăng nhập");
            }
            ViewBag.Company = find_company;
            return View(find_company);
        }
      
    }
}
