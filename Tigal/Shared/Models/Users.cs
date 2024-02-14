namespace Tigal.Shared.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string OtpCode { get; set; }
        public string ProfileImage{get;set;}
        public string Location{get;set;}
        public string Latitude{get;set;}
        public string Longitude{get;set;}
        public bool PolicyStatus{get;set;}
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
