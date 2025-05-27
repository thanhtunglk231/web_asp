namespace ReactApp1.Server.Models
{
    public class Reservation
    {
        public int ReservationID { get; set; }
        public string UserID { get; set; }
        public DateTime ReservationDate { get; set; }
        public string TimeSlot { get; set; }
        public int NumberOfGuests { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; } = "Đã đặt";

        public ApplicationUser User { get; set; }
    }

}
