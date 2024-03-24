using AspNetCoreHero.ToastNotification.Abstractions;
using BLOG_APPLICATION.Data;
using BLOG_APPLICATION.Models;
using BLOG_APPLICATION.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace BLOG_APPLICATION.Areas.Admin.Controllers
{

    [Area("admin")]
    [Authorize(Roles ="Admin")]
    public class SettingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public INotyfService _notification;
        public IWebHostEnvironment webHostEnvironment { get; }

        public SettingController(ApplicationDbContext context,INotyfService notification,IWebHostEnvironment WebHostEnvironment)
        {
            _notification = notification;
            webHostEnvironment = WebHostEnvironment;
            _context = context;
        }


        [HttpGet]
        public  async Task<IActionResult> Index()
        {
            var seting=_context.settings.ToList();  
            if(seting.Count > 0)
            {
                var setting = new SettingVM()
                {
                    Id = seting[0].Id,
                    Title = seting[0].Title,
                    shortDescription = seting[0].shortDescription,
                    siteName = seting[0].siteName,
                    ImageUrl = seting[0].ImageUrl,
                    facebookUrl = seting[0].facebookUrl,
                    twitterUrl = seting[0].twitterUrl,
                    githubUrl = seting[0].githubUrl,
                };
                return View(setting);
            }

            var setings = new Setting()
            {
                siteName = "Demo Name",
            };
            await _context.AddAsync(setings);
            await _context.SaveChangesAsync();  
            var createdsettings=_context.settings.ToList();
            var createdVM = new SettingVM()
            {
                Id= createdsettings[0].Id,
                Title = createdsettings[0].Title,
                shortDescription= createdsettings[0].shortDescription,
                facebookUrl= createdsettings[0].facebookUrl,
                twitterUrl= createdsettings[0].twitterUrl,
                githubUrl= createdsettings[0].githubUrl,
                ImageUrl= createdsettings[0].facebookUrl,
            };
           
            return View(createdVM);
        }
        [HttpPost]
        public async Task<IActionResult> Index(SettingVM settingVM)
        {
            if(!ModelState.IsValid)
            {
                return View(settingVM);
            }
            var settings = await _context.settings.FirstOrDefaultAsync(x => x.Id ==settingVM. Id);
            if (settings == null)
            {
                _notification.Error("settings does not exists");
                return View();
            }
             settings.Title = settingVM.Title;
            settings.siteName = settingVM.Title;
            settings.shortDescription=settingVM.shortDescription;
            settings.twitterUrl = settingVM.twitterUrl;
            settings.githubUrl = settingVM.githubUrl;
            settings.facebookUrl = settingVM.facebookUrl;

            if (settingVM.image != null)
            {
                settings.ImageUrl = uploadFile(settingVM.image);
            }
            await _context.SaveChangesAsync();
            _notification.Success("setting updated sucessfully");
            return RedirectToAction("Index","Setting",new {area="admin"});
            
        }


        private string uploadFile(IFormFile file)
        {
            string filename = "";
            var folderpath = Path.Combine(webHostEnvironment.WebRootPath, "Images");
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
