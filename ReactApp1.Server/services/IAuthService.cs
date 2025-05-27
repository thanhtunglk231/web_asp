// File: services/IAuthService.cs
using System.Threading.Tasks;
using ReactApp1.Server.Models;

namespace ReactApp1.Server.services
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(Loginuser loginuser);
        Task<bool> LoginUser(Loginuser loginuser);

        string GenerateTokenString(Loginuser user);
    }
}
