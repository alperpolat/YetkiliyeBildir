using Microsoft.AspNetCore.Mvc;
using YetkiliyeBildir.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace YetkiliyeBildir.Controllers
{
    public class ReportController : Controller
    {
        private readonly YetkiliyeBildirContext _context;
        private readonly IWebHostEnvironment _env;

        public ReportController(YetkiliyeBildirContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // İhbarları listele
        public async Task<IActionResult> Index()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            IQueryable<Report> reports = _context.Reports.OrderByDescending(r => r.CreatedAt);

            // Debug: Kullanıcı bilgilerini kontrol et
            Console.WriteLine($"Kullanıcı: {User.Identity.Name}");
            Console.WriteLine($"Admin mi: {User.IsInRole("Admin")}");
            Console.WriteLine($"YetkiliKurumId: {user?.YetkiliKurumId}");

            if (User.IsInRole("Admin"))
            {
                // Admin tüm ihbarları görebilir
                var adminReports = await reports.ToListAsync();
                Console.WriteLine($"Admin - Toplam ihbar sayısı: {adminReports.Count}");
                return View(adminReports);
            }
            else if (user != null && user.YetkiliKurumId != null)
            {
                // Kurum personeli sadece kendi kurumunun ihbarlarını görür
                reports = reports.Where(r => r.YetkiliKurumId == user.YetkiliKurumId);
                var kurumReports = await reports.ToListAsync();
                Console.WriteLine($"Kurum Personeli - Toplam ihbar sayısı: {kurumReports.Count}");
                return View(kurumReports);
            }
            else if (user != null)
            {
                // Vatandaş ise sadece kendi oluşturduğu ihbarları görsün
                reports = reports.Where(r => r.UserId == user.Id);
                var vatandasReports = await reports.ToListAsync();
                Console.WriteLine($"Vatandaş - Toplam ihbar sayısı: {vatandasReports.Count}");
                Console.WriteLine($"Vatandaş UserId: {user.Id}");
                return View(vatandasReports);
            }
            else
            {
                // Kullanıcı bulunamadıysa boş liste döndür
                Console.WriteLine("Kullanıcı bulunamadı!");
                return View(new List<Report>());
            }
        }

        // İhbar ekleme formu (GET)
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.Select(c => c.Name).ToList();
            ViewBag.Authorities = _context.Authorities.ToList();
            ViewBag.YetkiliKurumlar = _context.YetkiliKurumlar.ToList();
            return View();
        }

        // İhbar ekleme (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Report report, IFormFile? photo)
        {
            // Manuel validasyon ekle
            if (!report.AuthorityId.HasValue && !report.YetkiliKurumId.HasValue)
            {
                ModelState.AddModelError("", "Lütfen bir yetkili kurum seçiniz.");
            }
            else if (report.AuthorityId.HasValue && report.YetkiliKurumId.HasValue)
            {
                ModelState.AddModelError("", "Sadece bir kurum seçebilirsiniz.");
            }

            if (ModelState.IsValid)
            {
                // Kullanıcıyı bul ve ID'sini al
                var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                if (currentUser == null)
                {
                    ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                    ViewBag.Categories = _context.Categories.Select(c => c.Name).ToList();
                    ViewBag.Authorities = _context.Authorities.ToList();
                    ViewBag.YetkiliKurumlar = _context.YetkiliKurumlar.ToList();
                    return View(report);
                }

                if (photo != null && photo.Length > 0)
                {
                    var uploads = Path.Combine(_env.WebRootPath, "uploads");
                    if (!Directory.Exists(uploads))
                        Directory.CreateDirectory(uploads);
                    var fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
                    var filePath = Path.Combine(uploads, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }
                    report.PhotoPath = "/uploads/" + fileName;
                }
                
                report.CreatedAt = DateTime.Now;
                report.UserId = currentUser.Id; // Kullanıcının gerçek ID'sini ekle
                _context.Reports.Add(report);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            
            ViewBag.Categories = _context.Categories.Select(c => c.Name).ToList();
            ViewBag.Authorities = _context.Authorities.ToList();
            ViewBag.YetkiliKurumlar = _context.YetkiliKurumlar.ToList();
            return View(report);
        }
    }
} 