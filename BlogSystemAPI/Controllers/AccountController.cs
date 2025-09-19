using BlogSystemAPI.DTO;
using BlogSystemAPI.Models;
using BlogSystemAPI.Services;
using Microsoft.AspNetCore.Authorization;
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
        public AccountService Service { get; }

        public AccountController(AccountService service)
        {
            Service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
          
            var result = await Service.Register(registerDTO);

            if (!result.IsAuthenticated)
                return BadRequest(result.message);
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]LoginDTO loginDTO)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await Service.Login(loginDTO);
            if(!result.IsAuthenticated)
                return BadRequest(result.message);

            return Ok(result); 
        }

        [HttpPost("AddRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole([FromBody]RoleDTO roleDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await Service.AddRole(roleDTO);

            return (result != null) ? BadRequest(result) : Ok(roleDTO);
        }
    }
}