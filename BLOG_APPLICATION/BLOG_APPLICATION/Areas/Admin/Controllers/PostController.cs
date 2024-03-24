using AspNetCoreHero.ToastNotification.Abstractions;
using BLOG_APPLICATION.Data;
using BLOG_APPLICATION.Models;
using BLOG_APPLICATION.Utilities;
using BLOG_APPLICATION.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BLOG_APPLICATION.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize]
    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;

        public INotyfService Notify { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }
        public UserManager<ApplicationUser> userManager { get; }

        public PostController(ApplicationDbContext context, INotyfService notify,IWebHostEnvironment webHostEnvironment,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            Notify = notify;
            WebHostEnvironment = webHostEnvironment;
            this.userManager = userManager;
        }

        [HttpGet]
        public  async Task<IActionResult> Index()
        {
            var posts=new List<Post>();
            var loggedinuser = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
            var loggedinUserRole=await userManager.GetRolesAsync(loggedinuser!);
            if (loggedinUserRole[0] == WebRoles.WebAdmin)
            {
                 posts = await _context.post.Include(x=>x.User).ToListAsync();
            }
            else
            {
                 posts = await _context.post.Include(x => x.User).Where(x=>x.User.Id==loggedinuser!.Id).ToListAsync();
            }
            var listofPost = posts.Select(x => new PostVM()
            {
                Id = x.Id,
                Title = x.Title,
                AuthorName=x.User.FirstName +" " + x.User.LastName,
                createdDate=x.CreatedAt,
                ImageUrl=x.ImageUrl,
            }).ToList();
            return View(listofPost);
        }

        [HttpGet]
        public IActionResult CreatePost()
        {
            return View(new CreatePostDTO());
        }



        [HttpPost]
        public async Task< IActionResult> CreatePost(CreatePostDTO createPostDTO)
        {
            if(!ModelState.IsValid) { return View(createPostDTO); }

            //fetching loggedin user
            var logggedInuser =  userManager.Users.FirstOrDefault(x => x.UserName == User.Identity!.Name);
            var post = new Post();
            post.Title=createPostDTO.Title;
            post.Description=createPostDTO.Description;
            post.ShortDescription=createPostDTO.ShortDescription;
            post.ApplicationUserId = logggedInuser!.Id;

            if(createPostDTO.Title!=null)
            {
               var slug=createPostDTO.Title.Trim();
                slug = slug.Replace(" ", "_");
                post.Slug= slug +" -" + Guid.NewGuid();
            }
            if(createPostDTO.Image!=null)
            {
               post.ImageUrl= uploadFile(createPostDTO.Image);
            }
            await _context.post.AddAsync(post);
            await _context.SaveChangesAsync();
            Notify.Success("Post created sucessfully");


            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var post=await _context.post.FirstOrDefaultAsync(x => x.Id==id);

            var loggedinuser = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
            var loggedinUserRole = await userManager.GetRolesAsync(loggedinuser!);
            if (loggedinUserRole[0] == WebRoles.WebAdmin || loggedinuser!.Id == post!.ApplicationUserId)
            {
                _context.post.Remove(post!);
                await _context.SaveChangesAsync();
                Notify.Success("Post deleted sucessfully");
                return RedirectToAction("index", "Post", new { area = "admin" });
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var post= await _context.post.FirstOrDefaultAsync(x => x.Id==id);
            if (post == null)
            {
                Notify.Error("Post does not exists");
                return View();
            }


            var loggedinuser = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity!.Name);
            var loggedinUserRole = await userManager.GetRolesAsync(loggedinuser!);
            if (loggedinUserRole[0] != WebRoles.WebAdmin && loggedinuser!.Id == post!.ApplicationUserId)
            {
                Notify.Error("you are not authorized");
                return View();

            }
                var vm = new CreatePostDTO()
            {
                Id=post.Id,
                Title=post.Title,
                ShortDescription=post.ShortDescription,
                Description=post.Description,
                ImageUrl=post.ImageUrl,
                createdDate=post.CreatedAt,

            };
            return View(vm);
        }

        public async Task<IActionResult> Edit(CreatePostDTO post)
        {
            if(!ModelState.IsValid) { return View(post); }
            var posts=await _context.post.FirstOrDefaultAsync(x=>x.Id==post.Id);
            posts!.Title=post.Title;
            posts.ShortDescription=post.ShortDescription;
            posts.Description=post.Description;
            if (post.Image != null)
            {
                posts.ImageUrl = uploadFile(post.Image);
                await _context.SaveChangesAsync();
                Notify.Success("Post updated sucessfully");
                return RedirectToAction("Index", "Post");
            }
            return View();
        }


        private string uploadFile(IFormFile file)
        {
            string filename = "";
            var folderpath = Path.Combine(WebHostEnvironment.WebRootPath, "Images");
            filename=new Guid().ToString() +"-" +file.FileName;
            var fullpath=Path.Combine(folderpath,filename);
            using(FileStream filestream=System.IO.File.Create(fullpath))
            {
                file.CopyTo(filestream);
            }
            return filename;
        }
    }
}
