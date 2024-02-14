using System.ComponentModel.DataAnnotations;

namespace Tigal.Shared.Models.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
