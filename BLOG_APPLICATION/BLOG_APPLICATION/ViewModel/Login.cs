using System.ComponentModel.DataAnnotations;

namespace BLOG_APPLICATION.ViewModel
{
    public class Login
    {
        [Required]
        public string ?UserName { get; set;}

        [Required]
        public string ?Password { get; set;}

        
        public bool RememberMe {  get; set;}
    }
}
