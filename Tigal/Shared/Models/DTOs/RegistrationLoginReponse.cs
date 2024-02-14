namespace Tigal.Shared.DTOs.Models
{
    public class RegistrationLoginReponse
    {
        public string Message { get; set; }
        public string StatusCode { get; set; }
        public string Token { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public string JoinedOn { get; set; }
    }
}
