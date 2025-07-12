using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace YetkiliyeBildir.Models
{
    public class YetkiliyeBildirContext : IdentityDbContext<ApplicationUser>
    {
        public YetkiliyeBildirContext(DbContextOptions<YetkiliyeBildirContext> options) : base(options) { }

        public DbSet<Report> Reports { get; set; }
        public DbSet<Authority> Authorities { get; set; }
        public DbSet<YetkiliKurum> YetkiliKurumlar { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
} 