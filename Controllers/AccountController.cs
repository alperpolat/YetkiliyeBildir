using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using YetkiliyeBildir.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace YetkiliyeBildir.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly YetkiliyeBildirContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, YetkiliyeBildirContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string adSoyad, string email, string telefon, string password)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = email, Email = email, AdSoyad = adSoyad, Telefon = string.IsNullOrWhiteSpace(telefon) ? null : telefon };
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(email, password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GoogleLogin(string returnUrl = "/")
        {
            var redirectUrl = Url.Action("GoogleResponse", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return Challenge(properties, "Google");
        }

        public async Task<IActionResult> GoogleResponse(string returnUrl = "/")
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction("Login");

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
                return LocalRedirect(returnUrl);

            // Eğer kullanıcı yoksa yeni kullanıcı oluştur
            var email = info.Principal.FindFirstValue(System.Security.Claims.ClaimTypes.Email);
            if (email != null)
            {
                var user = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true };
                var createResult = await _userManager.CreateAsync(user);
                if (createResult.Succeeded)
                {
                    await _userManager.AddLoginAsync(user, info);
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
            }
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult YetkiliRegister()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> YetkiliRegister(string kurumAdi, string kurumMail, string kurumTelefon, string adSoyad, string telefon, string email, string password)
        {
            if (ModelState.IsValid)
            {
                // Kurum var mı kontrol et
                var kurum = await _context.YetkiliKurumlar.FirstOrDefaultAsync(k => k.KurumMail == kurumMail);
                if (kurum == null)
                {
                    kurum = new YetkiliKurum { KurumAdi = kurumAdi, KurumMail = kurumMail, KurumTelefon = kurumTelefon };
                    _context.YetkiliKurumlar.Add(kurum);
                    await _context.SaveChangesAsync();
                }
                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    AdSoyad = adSoyad,
                    Telefon = telefon,
                    YetkiliKurumId = kurum.Id,
                    CheckStatus = "Beklemede"
                };
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    // Official rolü başvuru onaylanınca atanacak
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("YetkiliBasvuruAlindi");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }

        public IActionResult YetkiliBasvuruAlindi()
        {
            return View();
        }
    }
} 