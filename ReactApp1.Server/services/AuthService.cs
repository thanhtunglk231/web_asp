using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ReactApp1.Server.Context;
using ReactApp1.Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReactApp1.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public AuthService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
        }

        public async Task<bool> RegisterUser(Loginuser loginuser)
        {
            var identityUser = new ApplicationUser
            {
                UserName = loginuser.username,
                Email = loginuser.username
            };

            var result = await _userManager.CreateAsync(identityUser, loginuser.password);
            return result.Succeeded;
        }

        public async Task<bool> LoginUser(Loginuser loginuser)
        {
            var identityUser = await _userManager.FindByEmailAsync(loginuser.username);
            if (identityUser == null)
                return false;

            return await _userManager.CheckPasswordAsync(identityUser, loginuser.password);
        }

        public async Task<string> GenerateTokenString(Loginuser loginUser)
        {
            // Tìm user theo username
            var userEntity = await _userManager.FindByNameAsync(loginUser.username);
            if (userEntity == null)
                throw new Exception("User không tồn tại");

            // Kiểm tra mật khẩu
            var passwordValid = await _userManager.CheckPasswordAsync(userEntity, loginUser.password);
            if (!passwordValid)
                throw new Exception("Mật khẩu không đúng");

            // Tạo claims
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userEntity.Id),
            new Claim(ClaimTypes.Email, userEntity.Email ?? ""),
            new Claim(ClaimTypes.Role, "Admin"), // hoặc lấy role thực tế
            new Claim(ClaimTypes.NameIdentifier, userEntity.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = _config.GetSection("Jwt:Key").Value;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signingCred
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
