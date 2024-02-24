using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace JWT_WEBAPI.Model
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email {  get; set; }
    }
}
