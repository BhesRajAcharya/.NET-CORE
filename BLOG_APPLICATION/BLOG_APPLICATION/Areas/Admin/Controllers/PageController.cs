using AspNetCoreHero.ToastNotification.Abstractions;
using BLOG_APPLICATION.Data;
using BLOG_APPLICATION.Models;
using BLOG_APPLICATION.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BLOG_APPLICATION.Areas.Admin.Controllers
{

    [Area("admin")]
    [Authorize(Roles ="Admin")]
    public class PageController : Controller
    {

        private readonly ApplicationDbContext _context;

        public INotyfService Notify { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }
        public PageController(ApplicationDbContext applicationDb,INotyfService notyfService,IWebHostEnvironment hostEnvironment)
        {
            this._context = applicationDb;
            this.Notify = notyfService;
            this.WebHostEnvironment = hostEnvironment;
            
        }

        [HttpGet]
      public async Task<IActionResult> About()
        {
            var page = await _context.page.FirstOrDefaultAsync(x => x.Slug == "About-Us");
            var vm= new PageVM()
            {
                Id = page!.Id,
                Title = page!.Title,
                ShortDescription = page!.ShortDescription,
                Description = page!.Description,
                ImageUrl = page!.ImageUrl,
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> About(PageVM pageVM)
        {
            if (!ModelState.IsValid) { return View(pageVM); }
            var page = await _context.page.FirstOrDefaultAsync(x => x.Slug == "About-Us");
            if (page == null)
            {
                Notify.Error("page  does not exists");
                return View();
            }
            page.Title=pageVM.Title;
            page.ShortDescription=pageVM.ShortDescription;
            page.Description=pageVM.Description;
            if (pageVM.Image != null)
            {
                pageVM.ImageUrl = uploadFile(pageVM.Image);
            }

            await _context.SaveChangesAsync();
            Notify.Success("Page updated  sucessfully");


            return RedirectToAction("About","Page", new {area="admin"});
            
        }

        [HttpGet]
        public async Task<IActionResult> Contact()
        {
            var page = await _context.page.FirstOrDefaultAsync(x => x.Slug == "Contact");
            var vm = new PageVM()
            {
                Id = page!.Id,
                Title = page!.Title,
                ShortDescription = page!.ShortDescription,
                Description = page!.Description,
                ImageUrl = page!.ImageUrl,
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Contact(PageVM pageVM)
        {
            if (!ModelState.IsValid) { return View(pageVM); }
            var page = await _context.page.FirstOrDefaultAsync(x => x.Slug == "Contact");
            if (page == null)
            {
                Notify.Error("page  does not exists");
                return View();
            }
            page.Title = pageVM.Title;
            page.ShortDescription = pageVM.ShortDescription;
            page.Description = pageVM.Description;
            if (pageVM.Image!= null)
            {
                pageVM.ImageUrl = uploadFile(pageVM.Image);
            }

            await _context.SaveChangesAsync();
            Notify.Success("Page updated  sucessfully");


            return RedirectToAction("Contact", "Page", new { area = "admin" });

        }

        [HttpGet]
        public async Task<IActionResult> Privacy()
        {
            var page = await _context.page.FirstOrDefaultAsync(x => x.Slug == "privacy");
            var vm = new PageVM()
            {
                Id = page!.Id,
                Title = page!.Title,
                ShortDescription = page!.ShortDescription,
                Description = page!.Description,
                ImageUrl = page!.ImageUrl,
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Privacy(PageVM pageVM)
        {
            if (!ModelState.IsValid) { return View(pageVM); }
            var page = await _context.page.FirstOrDefaultAsync(x => x.Slug == "privacy");
            if (page == null)
            {
                Notify.Error("page  does not exists");
                return View();
            }
            page.Title = pageVM.Title;
            page.ShortDescription = pageVM.ShortDescription;
            page.Description = pageVM.Description;
            if (pageVM.Image != null)
            {
                pageVM.ImageUrl = uploadFile(pageVM.Image);
            }

            await _context.SaveChangesAsync();
            Notify.Success("Page updated  sucessfully");


            return RedirectToAction("Privacy", "Page", new { area = "admin" });

        }



        private string uploadFile(IFormFile file)
        {
            string filename = "";
            var folderpath = Path.Combine(WebHostEnvironment.WebRootPath, "Images");
            filename = new Guid().ToString() + "-" + file.FileName;
            var fullpath = Path.Combine(folderpath, filename);
            using (FileStream filestream = System.IO.File.Create(fullpath))
            {
                file.CopyTo(filestream);
            }
            return filename;
        }
    }
}
