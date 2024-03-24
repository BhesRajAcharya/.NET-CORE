using System.ComponentModel.DataAnnotations;

namespace BLOG_APPLICATION.ViewModel
{
    public class ResetPassword
    {
        public  string ?Id { get; set; }
        public  string? UserName { get; set; }

        [Required]
        public string ? newPassword {  get; set; }

        [Compare(nameof(newPassword))]
        [Required]
        public string? ConfirmPassword { get; set; }
    }
}
