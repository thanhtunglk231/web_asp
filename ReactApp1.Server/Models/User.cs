using Microsoft.AspNetCore.Identity;

namespace ReactApp1.Server.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
