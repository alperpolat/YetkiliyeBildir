using Microsoft.AspNetCore.Mvc;
using YetkiliyeBildir.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace YetkiliyeBildir.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly YetkiliyeBildirContext _context;
        public AdminController(UserManager<ApplicationUser> userManager, YetkiliyeBildirContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // İhbarları listele
        public IActionResult Index()
        {
            var reports = _context.Reports.OrderByDescending(r => r.CreatedAt).ToList();
            return View(reports);
        }

        // İhbar detay
        public IActionResult Details(int id)
        {
            var report = _context.Reports.FirstOrDefault(r => r.Id == id);
            if (report == null) return NotFound();
            return View(report);
        }

        // Durum güncelle (GET)
        public IActionResult Edit(int id)
        {
            var report = _context.Reports.FirstOrDefault(r => r.Id == id);
            if (report == null) return NotFound();
            ViewBag.Statuses = System.Enum.GetValues(typeof(ReportStatus));
            return View(report);
        }

        // Durum güncelle (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReportStatus Status)
        {
            var report = _context.Reports.FirstOrDefault(r => r.Id == id);
            if (report == null) return NotFound();
            report.Status = Status;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> YetkiliBasvurular()
        {
            var bekleyenler = await _context.Users.Include(u => u.YetkiliKurum)
                .Where(u => u.CheckStatus == "Beklemede" && u.YetkiliKurumId != null)
                .ToListAsync();
            return View(bekleyenler);
        }

        [HttpPost]
        public async Task<IActionResult> YetkiliOnayla(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                user.CheckStatus = "Onaylandı";
                await _userManager.AddToRoleAsync(user, "Official");
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("YetkiliBasvurular");
        }

        [HttpPost]
        public async Task<IActionResult> YetkiliReddet(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                user.CheckStatus = "Reddedildi";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("YetkiliBasvurular");
        }

        [HttpGet]
        [Authorize(Roles = "Official")]
        public IActionResult KurumUserEkle()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Official")]
        public async Task<IActionResult> KurumUserEkle(string adSoyad, string telefon, string email, string password)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.YetkiliKurumId == null)
            {
                return Unauthorized();
            }
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                AdSoyad = adSoyad,
                Telefon = telefon,
                YetkiliKurumId = currentUser.YetkiliKurumId,
                CheckStatus = "Onaylandı"
            };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "KurumYetkilisi");
                return RedirectToAction("KurumUserEkle", new { success = true });
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        [Authorize(Roles = "Official")]
        public async Task<IActionResult> KurumPersonelleri()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.YetkiliKurumId == null)
                return Unauthorized();
            var personeller = await _context.Users
                .Where(u => u.YetkiliKurumId == currentUser.YetkiliKurumId && u.Id != currentUser.Id)
                .ToListAsync();
            return View(personeller);
        }

        [Authorize(Roles = "Official,KurumYetkilisi")]
        public async Task<IActionResult> KurumTalepleri()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || currentUser.YetkiliKurumId == null)
                return Unauthorized();
            var talepler = await _context.Reports
                .Where(r => r.AuthorityId == currentUser.YetkiliKurumId)
                .ToListAsync();
            return View(talepler);
        }
    }
} 