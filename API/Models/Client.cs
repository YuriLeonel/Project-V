using API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Client
    {
        [Key]
        public int IdClient { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public ClientType ClientType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
