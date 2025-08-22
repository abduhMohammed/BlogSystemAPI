using BlogSystemAPI.DTO;
using BlogSystemAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public AccountController(UserManager<ApplicationUser> usermanager, SignInManager<ApplicationUser> signInManager)
        {
            Usermanager = usermanager;
            SignInManager = signInManager;
        }

        public UserManager<ApplicationUser> Usermanager { get; }
        public SignInManager<ApplicationUser> SignInManager { get; }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterDTO registerDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                
                user.UserName = registerDTO.UserName;
                user.Email = registerDTO.Email;
                user.PasswordHash = registerDTO.Password;

                IdentityResult result = 
                        await Usermanager.CreateAsync(user, registerDTO.Password);

                if (result.Succeeded)
                {
                    return Created();
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("Password", item.Description);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]LoginDTO loginDTO)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? UserFromDB = 
                    await Usermanager.FindByNameAsync(loginDTO.Username);

                if (UserFromDB == null) return NotFound();

                bool found =
                        await Usermanager.CheckPasswordAsync(UserFromDB, UserFromDB.PasswordHash);
                if (found)
                {
                    #region Claims
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, UserFromDB.Id));
                    claims.Add(new Claim(ClaimTypes.Name, UserFromDB.UserName));

                    var UserRoles = await Usermanager.GetRolesAsync(UserFromDB);
                    foreach (var role in UserRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                    #endregion

                    #region Signing Credentials
                    string SecretKey = "this is my second project [BlogSystemAPI] in Web API";
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

                    var signCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    #endregion

                    #region Design Token
                    var token = new JwtSecurityToken (
                        claims: claims, 
                        expires: DateTime.Now.AddHours(1),
                        signingCredentials: signCred
                    );
                    #endregion

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok( new
                    {
                        tokenString,
                        Expiration = DateTime.UtcNow.AddHours(1)
                    });
                }
                ModelState.AddModelError("User Name", "User Name OR Password Invalid");
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest(new { message = "No user is logged in" });
            }

            SignInManager.SignOutAsync();
            return Ok(new {message = "Log Out Successfully"});
        }
    }
}