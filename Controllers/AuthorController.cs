using LibrarySystemApi.Data;
using LibrarySystemApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController(BookLibraryDbContext context) : ControllerBase
    {
        private readonly BookLibraryDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<List<Author>>> GetAuthors()
        {
            var authors = await _context.Authors.ToListAsync();
            return Ok(authors);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAuthorById(int id)
        {
            var author = await _context.Books.FindAsync(id);
            if(author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }
        [HttpPost]
        public async Task<ActionResult> AddAuthor(Author newAuthor)
        {
            if (newAuthor == null)
                return BadRequest();
            _context.Authors.Add(newAuthor);
            await _context.SaveChangesAsync();
           
            return CreatedAtAction(nameof(GetAuthorById), new {id = newAuthor.Id}, newAuthor);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, Author newAuthor)
        {
            var author =  await _context.Authors.FindAsync(id);
            if(author == null)
                return NotFound();
            author.Name = newAuthor.Name;
            author.Country = newAuthor.Country;
            await _context.SaveChangesAsync();  

            return NoContent();
            
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if(author == null)
                return NotFound();
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
