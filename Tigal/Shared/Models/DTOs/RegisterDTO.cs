using System.ComponentModel.DataAnnotations;

namespace Tigal.Shared.Models.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
