using BLOG_APPLICATION.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BLOG_APPLICATION.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> applicationUser {  get; set; }

        public DbSet<Page> page { get; set; }

        public DbSet<Post> post { get; set; }

        public DbSet<Setting> settings { get; set; }
    }
}
