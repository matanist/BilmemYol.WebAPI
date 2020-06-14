using System;
using System.ComponentModel.DataAnnotations;

namespace BilmemYol.WebAPI.Models
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
