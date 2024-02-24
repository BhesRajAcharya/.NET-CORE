using JWT_WEBAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT_WEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public UserManager<UserProfile> userManager { get; set; }
        public IConfiguration configuration { get; set; }

        public AuthenticationController(UserManager<UserProfile> userManager,IConfiguration configuration) { 
           this.userManager = userManager;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
       public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userexit=await userManager.FindByNameAsync(model.UserName);
            if (userexit != null)
            {
                return StatusCode(StatusCodes.Status208AlreadyReported, new Response { message="user already exist",status="error"});
            }
            UserProfile user = new UserProfile()
            {
                UserName = model.UserName,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var result=await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status403Forbidden,new Response { message="user creation failed", status="error"});
            }
            return StatusCode(StatusCodes.Status201Created,new Response { message="user created sucessfully",status ="error"});
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var user=await userManager.FindByNameAsync(loginModel.UserName);
            if(user!=null && await userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                var myclaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, loginModel.UserName ),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())

                };
                var siningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
                var token=new JwtSecurityToken(
                    issuer: configuration["JWT:ValidIssuer"],
                    audience: configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(2),
                    claims:myclaims,
                    signingCredentials:new SigningCredentials(siningKey,SecurityAlgorithms.HmacSha256));
                return Ok(new
                {
                    toekn = new JwtSecurityTokenHandler().WriteToken(token),
                });
                
            }
            return Unauthorized();
        }
    }
}
