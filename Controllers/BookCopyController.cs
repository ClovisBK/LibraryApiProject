using LibrarySystemApi.Data;
using LibrarySystemApi.Dtos;
using LibrarySystemApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookCopyController(BookLibraryDbContext context) : ControllerBase
    {
        private readonly BookLibraryDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<List<BookCopy>>> GetBookCopies()
        {
            var bookCopy = await _context.BooksCopies
                .Include(b => b.Book)
                .Select(b => new BookCopyDto
                {
                    Id = b.Id,
                    Bookid = b.BookId,
                    BookTitle = b.Book.Title,
                    CopyNumber = b.CopyNumber,
                    Barcode = b.Barcode,
                    Status = b.Status.ToString()

                }).ToListAsync();
            return Ok(bookCopy);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BookCopy>> GetBookCopyById(int id)
        {
            var existingCopy = await _context.BooksCopies
                .Include(b => b.Book)
                .Where(b => b.Id == id)
                .Select(b => new BookCopyDto {
                    Id = b.Id,
                    Bookid = b.BookId,
                    BookTitle = b.Book.Title,
                    CopyNumber = b.CopyNumber,
                    Barcode = b.Barcode,
                    Status = b.Status.ToString()
                }).FirstOrDefaultAsync();
            return Ok(existingCopy);
        }
        [HttpPost]
        public async Task<ActionResult> AddBookCopy(CreateBookCopyDto bookCopyDto)
        {
            if (bookCopyDto == null)
                return BadRequest();

            var bookCopy = new BookCopy
            {
                BookId = bookCopyDto.BookId,
                CopyNumber = bookCopyDto.CopyNumber,
                Barcode = bookCopyDto.Barcode,
                Status = BookCopyStatus.Available

            };
            await _context.BooksCopies.AddAsync(bookCopy);
            await _context.SaveChangesAsync();

            //returning the created book to the dto

            var bookDto = new BookCopyDto
            {
                Id = bookCopy.Id,
                Bookid = bookCopy.BookId,
                CopyNumber = bookCopy.CopyNumber,
                Barcode = bookCopy.Barcode,
            };
            return CreatedAtAction(nameof(GetBookCopyById), new { id = bookCopy.Id }, bookDto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBookCopy(int id, CreateBookCopyDto updatedBookCopyDto)
        {
            var existingBook = await _context.BooksCopies.FindAsync(id);
            if (existingBook == null)
                return NotFound("Book not found");
            existingBook.BookId = updatedBookCopyDto.BookId;
            existingBook.CopyNumber = updatedBookCopyDto.CopyNumber;
            existingBook.Barcode = updatedBookCopyDto.Barcode;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookCopy(int id)
        {
            var existingBook = await _context.BooksCopies.FindAsync(id);
            if (existingBook == null)
                return NotFound("Book copy not found");

            _context.BooksCopies.Remove(existingBook);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
