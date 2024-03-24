namespace BLOG_APPLICATION.ViewModel
{
    public class SettingVM
    {
            public int Id { get; set; }

            public string? Title { get; set; }

            public string? siteName { get; set; }

            public string? shortDescription { get; set; }

            public string? ImageUrl { get; set; }

            public string? facebookUrl { get; set; }

            public string? twitterUrl { get; set; }

            public string? githubUrl { get; set; }

        public IFormFile? image{ get; set;}
        }
    }


