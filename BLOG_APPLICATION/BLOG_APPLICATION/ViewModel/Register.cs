using System.ComponentModel.DataAnnotations;

namespace BLOG_APPLICATION.ViewModel
{
    public class Register
    {

        [Required]
        public string ?FirstName { get; set; }

        [Required]
        public string ?LastName { get; set; }

        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }


        public  bool IsAdmin{ get; set; }


    }
}
