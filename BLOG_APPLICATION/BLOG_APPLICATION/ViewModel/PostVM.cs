namespace BLOG_APPLICATION.ViewModel
{
    public class PostVM
    {
        public int Id { get; set; }
        public string ?Title { get; set; }

        public string? AuthorName { get; set; }

        public DateTime? createdDate { get; set; }

        public string? ImageUrl { get; set; }
    }
}
