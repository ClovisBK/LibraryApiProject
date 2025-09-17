namespace LibrarySystemApi.Dtos
{
    public class UpdateLoanDto
    {
        public int MemberId { get; set; }
        public int BookCopyId { get; set; }
        public DateTime DueDate { get; set; }
    }
}
