namespace YetkiliyeBildir.Models
{
    public class YetkiliKurum
    {
        public int Id { get; set; }
        public string KurumAdi { get; set; }
        public string KurumMail { get; set; }
        public string KurumTelefon { get; set; }
        public string? Description { get; set; }
        public string? ContactInfo { get; set; }
        public ICollection<ApplicationUser>? Users { get; set; }
    }
} 