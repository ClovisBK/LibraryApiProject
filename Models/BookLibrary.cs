using LibrarySystemApi.Models;

namespace LibrarySystemApi
{
    public class BookLibrary
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
        public int GenreId { get; set; }
        public Genre? Genre { get; set; }
        public int PublicationYear { get; set; }
        public string? Isbn  { get; set; }
        public int Pages { get; set; }
    }
}
