namespace LibrarySystemApi.Models
{
    public class Loan
    {
        public int Id  { get; set; }
        public int BookCopyId { get; set; }
        public BookCopy? BookCopy { get; set; }
        public int MemberId { get; set; }
        public Member? Member { get; set; }
        public DateTimeOffset LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTimeOffset? ReturnDate { get; set; }
        public int Renewals { get; set; } = 0;

    }
}
