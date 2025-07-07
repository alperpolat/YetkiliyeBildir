using Microsoft.AspNetCore.Identity;

namespace YetkiliyeBildir.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? AdSoyad { get; set; }
        public string? Telefon { get; set; }
        public int? YetkiliKurumId { get; set; }
        public string? CheckStatus { get; set; } // Beklemede, Onaylandı, Reddedildi
        public YetkiliKurum? YetkiliKurum { get; set; }
    }
} 