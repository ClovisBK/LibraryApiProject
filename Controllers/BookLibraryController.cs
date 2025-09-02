using LibrarySystemApi.Data;
using LibrarySystemApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookLibraryController : ControllerBase
    {
        private readonly BookLibraryDbContext _context;

        public BookLibraryController(BookLibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<BookDto>>> GetLibraryBooks()
        {
            var books = await _context.Books
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    ImageUrl = b.ImageUrl,
                    PublicationYear = b.PublicationYear,
                    Isbn = b.Isbn,
                    Pages = b.Pages,
                    AuthorId = b.AuthorId,
                    AuthorName = b.Author!.Name,
                    GenreName = b.Genre!.Name
                })
                .ToListAsync();

            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBookById(int id)
        {
            var book = await _context.Books
                .Where(b => b.Id == id)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    ImageUrl = b.ImageUrl,
                    PublicationYear = b.PublicationYear,
                    Isbn = b.Isbn,
                    Pages = b.Pages,
                    AuthorId = b.AuthorId,
                    GenreId = b.GenreId,
                    AuthorName = b.Author!.Name,
                    GenreName = b.Genre!.Name
                })
                .FirstOrDefaultAsync();

            if (book == null)
                return NotFound();

            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult> AddBook(CreateBookDto newBookDto)
        {
            // Check author exists
            var authorExists = await _context.Authors.AnyAsync(a => a.Id == newBookDto.AuthorId);
            if (!authorExists)
                return BadRequest("Author does not exist.");
            //check Genre exists
            var GenreExists = await _context.Genres.AnyAsync(g => g.Id == newBookDto.GenreId);
            if (!GenreExists)
                return BadRequest("Genre not found");

            var book = new BookLibrary
            {
                Title = newBookDto.Title,
                ImageUrl = newBookDto.ImageUrl,
                PublicationYear = newBookDto.PublicationYear,
                Isbn = newBookDto.Isbn,
                Pages = newBookDto.Pages,
                AuthorId = newBookDto.AuthorId,
                GenreId = newBookDto.GenreId
                
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            // Returning the created book as a dto
            var bookDto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                ImageUrl = book.ImageUrl,
                PublicationYear = book.PublicationYear,
                Isbn = book.Isbn,
                Pages = book.Pages,
                AuthorId = book.AuthorId,
                GenreId= book.GenreId
            };

            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, bookDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, UpdateBookDto updatedBookDto)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound();

            // Check if new AuthorId exists
            var authorExists = await _context.Authors.AnyAsync(a => a.Id == updatedBookDto.AuthorId);
            if (!authorExists)
                return BadRequest("Author does not exist.");
            var genereExists = await _context.Genres.AnyAsync(g => g.Id == updatedBookDto.GenreId);
            if (!genereExists)
                return BadRequest("Genre does not exist");

            book.Title = updatedBookDto.Title;
            book.ImageUrl = updatedBookDto.ImageUrl;
            book.PublicationYear = updatedBookDto.PublicationYear;
            book.Isbn = updatedBookDto.Isbn;
            book.Pages = updatedBookDto.Pages;
            book.AuthorId = updatedBookDto.AuthorId;
            book.GenreId = updatedBookDto.GenreId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return NotFound();

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("Search")]
        public async Task<ActionResult<List<BookDto>>> SearchBookByTitle([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return BadRequest("Cannot be empty");
            }
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Where(b => b.Title != null && b.Title.Contains(term.ToLower()))
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    ImageUrl = b.ImageUrl,
                    AuthorId = b.AuthorId,
                    AuthorName = b.Author!.Name,
                    GenreName  = b.Genre!.Name,
                    PublicationYear = b.PublicationYear,
                    Isbn = b.Isbn,
                    Pages = b.Pages
                }).ToListAsync();
            return Ok(books);
        }
        [HttpGet("publicationYear")]
        public async Task<ActionResult<List<BookDto>>> GetBooksByPublicationYear(int year)
        {
            if (year <= 0)
                return BadRequest("Invalid year");
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .Where(b => b.PublicationYear == year)
                .Select(b => new BookDto
                {
                    Id= b.Id,
                    Title = b.Title,
                    ImageUrl = b.ImageUrl,
                    AuthorId = b.AuthorId,
                    PublicationYear = b.PublicationYear,
                    Isbn = b.Isbn,
                    Pages = b.Pages,
                    AuthorName = b.Author!.Name
                }).ToListAsync();
            return Ok(books);
        }
        [HttpGet("Order-by-year")]
        public async Task<ActionResult<List<BookDto>>> GetBooksOrderedByYear()
        {
            var orderedBooks = await _context.Books
                .OrderBy(b => b.PublicationYear)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    ImageUrl = b.ImageUrl,
                    AuthorId = b.AuthorId,
                    PublicationYear = b.PublicationYear,
                    Isbn = b.Isbn,
                    Pages = b.Pages,
                    AuthorName = b.Author!.Name,
                    GenreName = b.Genre!.Name
                }).ToListAsync();
            return Ok(orderedBooks);
        }
    }
}
