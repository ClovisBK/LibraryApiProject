namespace LibrarySystemApi.Dtos
{
    public class CreateBookCopyDto
    {
        public int BookId { get; set; }
        public string CopyNumber { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
    }
}
