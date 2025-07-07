using System.ComponentModel.DataAnnotations;

namespace YetkiliyeBildir.Models
{
    public class Authority
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? ContactInfo { get; set; }
    }
} 