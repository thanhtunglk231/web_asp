using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ReactApp1.Server.Context;
using ReactApp1.Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReactApp1.Server.services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;  // Sửa ở đây
        private readonly IConfiguration _config;

        public AuthService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
        }

        public async Task<bool> RegisterUser(Loginuser loginuser)
        {
            var identityUser = new ApplicationUser   // Sửa ở đây
            {
                UserName = loginuser.username,
                Email = loginuser.username,
            };

            var result = await _userManager.CreateAsync(identityUser, loginuser.password);
            return result.Succeeded;
        }

        public async Task<bool> LoginUser(Loginuser loginuser)
        {
            var identityuser = await _userManager.FindByEmailAsync(loginuser.username);
            if (identityuser == null)
            {
                return false;
            }

            return await _userManager.CheckPasswordAsync(identityuser, loginuser.password);
        }

        public string GenerateTokenString(Loginuser user)
        {
            var key = _config.GetSection("Jwt:Key").Value;
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("JWT Key is not configured.");
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.username),
                new Claim(ClaimTypes.Email, user.username),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.NameIdentifier, user.username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

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
