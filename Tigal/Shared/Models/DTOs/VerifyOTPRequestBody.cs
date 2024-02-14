using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tigal.Shared.Models.DTOs
{
    public class VerifyOTPRequestBody
    {
        public string Phone { get; set; }
        public string Otp { get; set; }
    }
}
