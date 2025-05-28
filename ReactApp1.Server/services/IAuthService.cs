// File: services/IAuthService.cs
using System.Threading.Tasks;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(Loginuser loginuser);
        Task<bool> LoginUser(Loginuser loginuser);

        Task<string> GenerateTokenString(Loginuser user);
    }
}
