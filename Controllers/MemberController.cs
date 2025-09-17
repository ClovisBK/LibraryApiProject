using System.Security.Claims;
using LibrarySystemApi.Data;
using LibrarySystemApi.Dtos;
using LibrarySystemApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Identity.Client;

namespace LibrarySystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController(BookLibraryDbContext context) : ControllerBase
    {
        private readonly BookLibraryDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<List<Member>>> GetMembers()
        {
            var members = await _context.Members.ToListAsync();
            return Ok(members);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetMemberById(int id)
        {
            var existingMembers = await _context.Members.FindAsync(id);
            if (existingMembers == null) 
                return NotFound($"Member is not found");
            return Ok(existingMembers);
        }
       
        [HttpPut("editProfile")]
        public async Task<IActionResult> UpdateMember(UpdateMemberDto updatedMemberDto)
        {
            
            var userClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userClaim == null)
                return Unauthorized("User is not active");
            var userId = Guid.Parse(userClaim);

            var existingMember = await _context.Members.FirstOrDefaultAsync(u => u.UserId == userId);
            if (existingMember == null)
                return Unauthorized("Sorry you are not allowed");
            existingMember.FullName = updatedMemberDto.FullName;
            existingMember.Email = updatedMemberDto.Email;
            existingMember.Phone = updatedMemberDto.Phone;
            existingMember.Address = updatedMemberDto.Address;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var existingMember = await _context.Members.FindAsync(id);
            if(existingMember == null)
                return NotFound();
            _context.Members.Remove(existingMember);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [Authorize(Roles = "Member")]
        [HttpGet("{id}/borrowHistory")]
        public async Task<ActionResult<List<LoanDto>>> GetBorrowedBooksForMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
                return NotFound("Member not found");
            var loans = await _context.Loans
                .Where(l => l.MemberId == id)
                .Include(b => b.BookCopy)
                    .ThenInclude(b => b.Book)
                .Select(h => new LoanDto
                {
                    BookTitle = h.BookCopy.Book.Title,
                    CopyNumber = h.BookCopy.CopyNumber,
                    LoanDate = h.LoanDate,
                    DueDate = h.DueDate,
                    Renewal = h.Renewals

                }).ToListAsync();
            return Ok(loans);
        }
    }
}
