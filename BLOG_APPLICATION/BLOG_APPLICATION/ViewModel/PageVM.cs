﻿using System.ComponentModel.DataAnnotations;

namespace BLOG_APPLICATION.ViewModel
{
    public class PageVM
    {
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl{ get; set; }

        public IFormFile? Image { get; set; }
    }
}
