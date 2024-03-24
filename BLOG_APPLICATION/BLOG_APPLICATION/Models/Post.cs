namespace BLOG_APPLICATION.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string ? ShortDescription {  get; set; }

        public string ApplicationUserId {  get; set; }

        public ApplicationUser User { get; set; }

        public DateTime ?CreatedAt { get; set; }= DateTime.Now;

        public string? Description { get; set; } 

        public string ?Slug {  get; set; }

        public string? ImageUrl { get; set; }


    }
}
