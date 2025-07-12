using System;
using System.ComponentModel.DataAnnotations;

namespace YetkiliyeBildir.Models
{
    public enum ReportStatus
    {
        Bekliyor,
        Alindi,
        Cozuldu
    }

    public class Report
    {
        public int Id { get; set; }

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public string? PhotoPath { get; set; }

        public ReportStatus Status { get; set; } = ReportStatus.Bekliyor;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int? AuthorityId { get; set; }
        public Authority? Authority { get; set; }
        public int? YetkiliKurumId { get; set; }
        public YetkiliKurum? YetkiliKurum { get; set; }

        public string FirmaAksiyon { get; set; } // İşlemeAlındı, Reddedildi, Tamamlandı
        public string FirmaNotu { get; set; }
    }
} 