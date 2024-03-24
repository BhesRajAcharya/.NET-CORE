using BLOG_APPLICATION.Data;
using BLOG_APPLICATION.Models;
using Microsoft.AspNetCore.Identity;

namespace BLOG_APPLICATION.Utilities
{
    public class DbInitializer : IDbinitializer
    {
        public readonly ApplicationDbContext _Context;
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext _Context, UserManager<ApplicationUser> _userManager,RoleManager<IdentityRole> _roleManager)
        {
            this._Context = _Context;
            this._userManager = _userManager;
            this._roleManager = _roleManager;
        }
        public void Initialize()
        {

            if(! _roleManager.RoleExistsAsync(WebRoles.WebAdmin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebRoles.WebAdmin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebRoles.WebAuthor)).GetAwaiter().GetResult();
                _userManager.CreateAsync(new ApplicationUser()
                {
                    UserName = "Adminbij",
                    Email = "admin@gmail.com",
                    FirstName = "Bijaya",
                    LastName = "Acharya"
                }, "Admins@1234").Wait();

                var user=_Context.applicationUser.FirstOrDefault(x=>x.Email == "admin@gmail.com");
                if (user != null)
                {
                    _userManager.AddToRoleAsync(user,WebRoles.WebAdmin).GetAwaiter().GetResult();
                }
                var Aboutpage = new Page()
                {
                    Title = "About",
                    Slug = "About-Us"
                };

                var Contactpage = new Page()
                {
                    Title = "Contact",
                    Slug = "Contact"
                };

                var privacypolicypage = new Page()
                {
                    Title = "Privacy",
                    Slug = "privacy"
                };
                _Context.page.Add(privacypolicypage);
                _Context.page.Add(Aboutpage);
               _Context.page.Add(Contactpage);
                _Context.SaveChanges();


            }
            
        }
    }
}
