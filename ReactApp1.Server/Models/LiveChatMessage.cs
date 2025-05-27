using System.ComponentModel.DataAnnotations.Schema;

namespace ReactApp1.Server.Models
{
    public class LiveChatMessage
    {
        public int MessageID { get; set; }
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
        public string MessageText { get; set; }
        public DateTime SentAt { get; set; } = DateTime.Now;
        [Column(TypeName = "NUMBER(1)")]
        public bool IsRead { get; set; } = false;

        public ApplicationUser Sender { get; set; }
        public ApplicationUser Receiver { get; set; }
    }

}
