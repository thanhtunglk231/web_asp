using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Context;
using ReactApp1.Server.Models;
using ReactApp1.Server.Services;
using System.Numerics;
using System.Security.Claims;
namespace ReactApp1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authservice;
        private readonly ApplicationDbContext _applicationDb;
        public AuthController(IAuthService authservice,ApplicationDbContext context)
        {
            _authservice = authservice;
            _applicationDb = context;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> registeruser([FromBody] Loginuser loginuser)
        {
            if( await _authservice.RegisterUser(loginuser))
            {
                return Ok("Dang ky thanh cong");
            }
            return BadRequest("Dang ky that bai");
        }


        [HttpPost("Login")]
        public async Task<IActionResult> login([FromBody] Loginuser loginuser)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            Console.WriteLine("User ID từ token: " + userId);
            if (await _authservice.LoginUser(loginuser))
            {
                var tokenstring = await _authservice.GenerateTokenString(loginuser);
                return Ok(new { token = tokenstring });
            }
            return BadRequest("Đăng nhập thất bại");
        }


    }
}
