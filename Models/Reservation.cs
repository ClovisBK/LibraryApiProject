using System.ComponentModel;

namespace LibrarySystemApi.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public  Book? Book { get; set; }
        public int MemberId { get; set; }
        public Member? Member { get; set; }
        public DateTimeOffset ReservationDate { get; set; }
        public ReservationSatus Status { get; set; }

    }
    public enum ReservationSatus
    {
        Pending,
        [Description("Ready for pickup")]
        ReadyForPickup,
        Expired,
        Completed
    }
}
