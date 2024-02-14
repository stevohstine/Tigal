using Microsoft.AspNetCore.Http;

namespace Tigal.Server.Models
{
    public class UpdateProfilePhoto
    {
        public string phoneNumber{get;set;}
        public IFormFile profilePicture { get; set; }
    }
}