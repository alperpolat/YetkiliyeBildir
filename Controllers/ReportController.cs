using Microsoft.AspNetCore.Mvc;
using YetkiliyeBildir.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Linq;

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
        public IActionResult Index()
        {
            var reports = _context.Reports.OrderByDescending(r => r.CreatedAt).ToList();
            return View(reports);
        }

        // İhbar ekleme formu (GET)
        public IActionResult Create()
        {
            ViewBag.Categories = new[]
            {
                "Terk edilmiş scooter/bisiklet",
                "Çukur, bozuk yol, kaldırım",
                "Yanık sokak lambası",
                "Aşırı dolu çöp kutusu",
                "Hasarlı trafik tabelası",
                "Güvensiz yapı/bariyer",
                "Kaybolmuş hayvan"
            };
            ViewBag.Authorities = _context.Authorities.ToList();
            ViewBag.YetkiliKurumlar = _context.YetkiliKurumlar.ToList();
            return View();
        }

        // İhbar ekleme (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Report report, IFormFile? photo, int? selectedAuthorityId, int? selectedYetkiliKurumId)
        {
            if (ModelState.IsValid)
            {
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
                if (selectedAuthorityId.HasValue && selectedAuthorityId.Value > 0)
                {
                    report.AuthorityId = selectedAuthorityId;
                    report.YetkiliKurumId = null;
                }
                else if (selectedYetkiliKurumId.HasValue && selectedYetkiliKurumId.Value > 0)
                {
                    report.YetkiliKurumId = selectedYetkiliKurumId;
                    report.AuthorityId = null;
                }
                _context.Reports.Add(report);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new[]
            {
                "Terk edilmiş scooter/bisiklet",
                "Çukur, bozuk yol, kaldırım",
                "Yanık sokak lambası",
                "Aşırı dolu çöp kutusu",
                "Hasarlı trafik tabelası",
                "Güvensiz yapı/bariyer",
                "Kaybolmuş hayvan"
            };
            ViewBag.Authorities = _context.Authorities.ToList();
            ViewBag.YetkiliKurumlar = _context.YetkiliKurumlar.ToList();
            return View(report);
        }
    }
} 