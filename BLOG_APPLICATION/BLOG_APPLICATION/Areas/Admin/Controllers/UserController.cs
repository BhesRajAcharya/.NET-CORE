using AspNetCoreHero.ToastNotification.Abstractions;
using BLOG_APPLICATION.Models;
using BLOG_APPLICATION.Utilities;
using BLOG_APPLICATION.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace BLOG_APPLICATION.Areas.Admin.Controllers
{
    [Area("admin")]
    public class UserController : Controller
    {
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly SignInManager<ApplicationUser> _signInManager;
        public readonly INotyfService _notification;

        public UserController(UserManager<ApplicationUser > _userManager, SignInManager<ApplicationUser> _signInManager, INotyfService _notification)
        {
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            this._notification = _notification;

        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        public  async Task<IActionResult> Index()
        {
            var users= await _userManager.Users.ToListAsync();
            var dto = users.Select(x => new UserDto()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                UserName = x.UserName,
                Email = x.Email,
            }).ToList();

            foreach(var user in dto)
            {
                var singleuser=await _userManager.FindByIdAsync(user.Id);  
                var role=await _userManager.GetRolesAsync(singleuser);
                user.Role=role.FirstOrDefault();
            }
            return View(dto);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task< IActionResult> ResetPassword(string id)
        {
            var user=await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _notification.Error("user does not exist");
                return View();
            }
            var restuser = new ResetPassword()
            {
                Id = user.Id,
                UserName = user.UserName,
            };
            return View(restuser);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(resetPassword.Id);
                if (user == null)
                {
                    _notification.Error("user does not exist");
                    return View(resetPassword);
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, resetPassword.newPassword);
                if (result.Succeeded)
                {
                    _notification.Success("password reset sucessfully");
                    return RedirectToAction("Index", "User");
                }

                
            }
            return View(resetPassword);
        }


        [Authorize(Roles ="Admin")]
        [HttpGet]
        public IActionResult Register()
        {
            return View(new Register());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public  async Task<IActionResult > Register( Register register)
        {
            if (!ModelState.IsValid) { return View(register); }
              var emailverify = await  _userManager.FindByEmailAsync(register.Email);
            if (emailverify != null)
            {
                _notification.Error("Email already exist");
                return View(register);
            }
            var verifyUsername = await _userManager.FindByNameAsync(register.UserName);
            if (verifyUsername != null)
            {
                _notification.Error("Username already exist");
                return View(register);
            }


            var user = new ApplicationUser()
            {
                FirstName = register.UserName,
                LastName = register.LastName,
                Email = register.Email,
                UserName = register.UserName,
            };
            var result = await _userManager.CreateAsync(user, register.Password);
            if (result.Succeeded)
            {
                if (register.IsAdmin)
                {
                    await _userManager.AddToRoleAsync(user, WebRoles.WebAdmin);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user,WebRoles.WebAuthor);
                }
                _notification.Success("user created sucessfully");
                return RedirectToAction("Index", "User", new { area = "admin" });
            }
            return View(register);
        }

        [HttpGet("login")]
        public IActionResult Login()
        {

            if (!HttpContext.User.Identity!.IsAuthenticated)
            {
                return View(new Login());
            }

            return RedirectToAction("Index", "Post", new { area = "admin" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login login)
        {
            if (!ModelState.IsValid) { return View(); }
            var userexist = await _userManager.FindByNameAsync(login.UserName);
            if (userexist == null)
            {
                _notification.Error("username is not exist");
                return View();

            }
            var verifypassowrd = await _userManager.CheckPasswordAsync(userexist, login.Password);

            if (!verifypassowrd)
            {
                _notification.Error("password does not match");
                return View();
            }

            await _signInManager.PasswordSignInAsync(login.UserName, login.Password, login.RememberMe, true);
            _notification.Success("login sucessfully");

            return RedirectToAction("Index", "Post", new { area = "admin" });

        }

        [HttpPost]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            _notification.Success("you are logged out sucessfully");
            return RedirectToAction("Index", "Home", new { area = " " });
        }

        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
    }
