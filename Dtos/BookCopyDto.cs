namespace LibrarySystemApi.Dtos
{
    public class BookCopyDto
    {
        public int Id { get; set; }
        public int Bookid { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string CopyNumber { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

    }
}
