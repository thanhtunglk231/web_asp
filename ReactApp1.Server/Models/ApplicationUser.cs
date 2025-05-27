using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models
{
    public class ApplicationUser : IdentityUser
    {


        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<LiveChatMessage> SentMessages { get; set; }
        public ICollection<LiveChatMessage> ReceivedMessages { get; set; }
    }
}
