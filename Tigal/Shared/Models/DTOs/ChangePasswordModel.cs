namespace Tigal.Shared.Models.DTOs
{
    public class ChangePasswordModel
    {
        public string phoneNumber { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}
