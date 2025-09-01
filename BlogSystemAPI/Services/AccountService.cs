using BlogSystemAPI.DTO;
using BlogSystemAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystemAPI.Services
{
    public class AccountService
    {
        public AccountService
            (UserManager<ApplicationUser> usermanager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager)
        {
            Usermanager = usermanager;
            RoleManager = roleManager;
            SignInManager = signInManager;
        }

        public UserManager<ApplicationUser> Usermanager { get; }
        public RoleManager<IdentityRole> RoleManager { get; }
        public SignInManager<ApplicationUser> SignInManager { get; }

        public async Task<AuthDTO> Register(RegisterDTO dto)
        {
            var existingUser = await Usermanager.FindByNameAsync(dto.UserName);
            var existingEmail = await Usermanager.FindByEmailAsync(dto.Email);

            if (existingUser != null || existingEmail != null)
                return new AuthDTO { message = "User or Email Already registerd" };

            ApplicationUser user = new ApplicationUser() {
                UserName = dto.UserName,
                Email = dto.Email
            };

            IdentityResult result = await Usermanager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var item in result.Errors)
                {
                    errors += item.Description + " ";
                }
            }

            if (await RoleManager.RoleExistsAsync("User"))
            {
                await Usermanager.AddToRoleAsync(user, "User");
            }

            return new AuthDTO
            {
                message = "User Registered Successfully",
                username = user.UserName,
                Email = user.Email,
                IsAuthenticated = true,
                Roles = new List<string> { "User" }
            };
        }

        public async Task<AuthDTO> Login(LoginDTO dto)
        {
            ApplicationUser UserFromDB = await Usermanager.FindByNameAsync(dto.Username);

            if (UserFromDB == null || !await Usermanager.CheckPasswordAsync(UserFromDB, dto.Password))
            {
                return new AuthDTO
                {
                    message = "Invalid Username or Password"
                };
            }

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
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signCred
            );
            #endregion

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthDTO 
            {
                message = "Login Successfull",
                username = UserFromDB.UserName,
                Email = UserFromDB.Email,
                IsAuthenticated = true,
                Roles = UserRoles.ToList(),
                token = tokenString,
                ExpiresOn = DateTime.UtcNow.AddHours(1)
            };
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                await SignInManager.SignOutAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}