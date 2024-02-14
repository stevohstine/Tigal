namespace Tigal.Shared.Models.DTOs
{
    public class ResetPasswordModel
    {
        public string phoneNumber { get; set; }
        public string otpCode { get; set; }
        public string password { get; set; }
    }
}
