namespace LibrarySystemApi.Dtos
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public DateTimeOffset ReservationDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
