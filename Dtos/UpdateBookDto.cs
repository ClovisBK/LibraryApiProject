namespace LibrarySystemApi.Dtos
{
    public class UpdateBookDto
    {
        public string? Title { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public string? Isbn { get; set; }
        public int Pages { get; set; }
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
    }
}
