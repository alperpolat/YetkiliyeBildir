using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YetkiliyeBildir.Models;

namespace YetkiliyeBildir.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly YetkiliyeBildirContext _context;

    public HomeController(ILogger<HomeController> logger, YetkiliyeBildirContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Son 5 bildirimi al
        var sonBildirimler = await _context.Reports
            .OrderByDescending(r => r.CreatedAt)
            .Take(5)
            .Select(r => new
            {
                r.Id,
                r.Category,
                r.Description,
                r.Latitude,
                r.Longitude,
                r.Status,
                r.CreatedAt,
                r.PhotoPath
            })
            .ToListAsync();

        // Harita için tüm bildirimlerin konumlarını al (anonim)
        var haritaVerileri = await _context.Reports
            .Where(r => r.Latitude.HasValue && r.Longitude.HasValue)
            .Select(r => new
            {
                r.Latitude,
                r.Longitude,
                r.Status,
                r.Category
            })
            .ToListAsync();

        ViewBag.SonBildirimler = sonBildirimler;
        ViewBag.HaritaVerileri = haritaVerileri;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
