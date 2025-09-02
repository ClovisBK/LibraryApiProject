namespace LibrarySystemApi.Dtos
{
    public class CreateLoanDto
    {
        public int MemberId { get; set; }
        public int BookCopyId { get; set; }
        public DateTime DueDate { get; set; }

    }
}
