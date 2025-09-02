using LibrarySystemApi.Models;

namespace LibrarySystemApi.Dtos
{
    public class LoanDto
    {
        public int Id { get; set; }
        public int BookCopyId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string CopyNumber { get; set; } = string.Empty;
        public int MemberId { get; set; }
        public string Borrower { get; set; } = string.Empty;
        public DateTimeOffset LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTimeOffset? ReturnDate { get; set; }
        public int Renewal { get; set; }
    }
}
