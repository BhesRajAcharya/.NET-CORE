using BLOG_APPLICATION.Data;
using BLOG_APPLICATION.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BLOG_APPLICATION.Controllers
{
    public class PageController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PageController(ApplicationDbContext context)
        {
            this._context = context;
            
        }
        public  async Task<IActionResult> About()
        {
            var page=await _context.page.FirstOrDefaultAsync(x=>x.Slug=="About-Us");
            var vm = new PageVM()
            {
                Title = page!.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ImageUrl = page.ImageUrl
            };
            return View(vm);
        }

        public async Task< IActionResult> Contact()
        {
            var page = await _context.page.FirstOrDefaultAsync(x => x.Slug == "Contact");
            var vm = new PageVM()
            {
                Title = page!.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ImageUrl = page.ImageUrl,
            };
            return View(vm);
        }

        public async Task< IActionResult> PrivacyPolicy()
        {
            var page = await _context.page.FirstOrDefaultAsync(x => x.Slug == "privacy");
            var vm = new PageVM()
            {
                Title = page!.Title,
                ShortDescription = page.ShortDescription,
                Description = page.Description,
                ImageUrl = page.ImageUrl,
            };
            return View(vm);
        }
    }
}
