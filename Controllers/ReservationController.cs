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
    public class ReservationController(BookLibraryDbContext context) : ControllerBase
    {
        private readonly BookLibraryDbContext _context = context;

        [HttpGet]
        public async Task<ActionResult<List<Reservation>>> GetReservations()
        {
            await CleanupExpiredReservationsInternal();

            var reservation = await _context.Reservations
                .Include(b => b.Book)
                .Include(m => m.Member)
                .Select(r => new ReservationDto
                {
                    Id = r.Id,
                    BookId = r.BookId,
                    BookTitle = r.Book.Title,
                    MemberId = r.MemberId,
                    ReservationDate = r.ReservationDate,
                    Status = r.Status.ToString()
                }).ToListAsync();

            return Ok(reservation);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationDto>> GetReservationById(int id)
        {
            var existingReservation = await _context.Reservations
                .Where(l => l.Id == id)
                .Include(b => b.Book)
                .Include(m => m.Member)
                .Select(s => new ReservationDto
                {
                    Id = s.Id,
                    BookId = s.BookId,
                    MemberId= s.MemberId,
                    BookTitle = s.Book.Title,
                    ReservationDate = s.ReservationDate,
                    Status = s.Status.ToString()
                }).FirstOrDefaultAsync();
            if (existingReservation == null)
                return NotFound();

            return Ok(existingReservation);


        }

        [HttpPost]
        public async Task<ActionResult> AddReservation(CreateReservationDto newReservation)
        {
            if (newReservation == null)
                return BadRequest("Invalid Reservation Data");

            var memberExists = await _context.Members.AnyAsync(m => m.Id == newReservation.MemberId);
            if (!memberExists)
                return NotFound("Member not found");
            var bookExists = await _context.Books.AnyAsync(b => b.Id == newReservation.BookId);
            if (!bookExists)
                return NotFound("Book not found");

            //checking for available copies
            var isAvailable = await _context.BooksCopies
                .AnyAsync(bc => bc.BookId == newReservation.BookId && bc.Status == BookCopyStatus.Available);
            if (isAvailable)
                return BadRequest("An available copy of this book already exists. No reservation needed");

            //checking if member already has a reservation for the book
            var alreadyHasReservation = await _context.Reservations
                .AnyAsync(r => r.MemberId == newReservation.MemberId && r.BookId == newReservation.BookId && r.Status == ReservationSatus.Pending);
            if (alreadyHasReservation)
                return BadRequest("Member already has a pending reservation on this book");
            
            //checking if the member already has the book on loan
            var hasActiveLoan = await _context.Loans
                .Where(l => l.MemberId == newReservation.MemberId && !l.ReturnDate.HasValue)
                .Include(l => l.BookCopy)
                .AnyAsync( l => l.BookCopy!.BookId == newReservation.BookId );

            if(hasActiveLoan)
                return BadRequest("Member has an active loan on this book. Cannot be reserved");

            var reservation = new Reservation
            {
                BookId = newReservation.BookId,
                MemberId = newReservation.MemberId,
                ReservationDate = DateTimeOffset.Now,
                Status = ReservationSatus.Pending
            };
            
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReservationById), new {id = reservation.Id}, reservation);
                
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var existingReservation = await _context.Reservations.FindAsync(id);
            if (existingReservation == null)
                return NotFound("Reservation not found");
            _context.Reservations.Remove(existingReservation);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("cleanup")]
        public async Task<IActionResult> CleanupExpiredReservations()
        {
            await CleanupExpiredReservationsInternal();
            return Ok("Expired reservations have been processed");
        }
        private async Task CleanupExpiredReservationsInternal()
        {
            var expiredReservations = await _context.Reservations
                .Where(r => r.Status == ReservationSatus.ReadyForPickup && r.ReservationDate.AddDays(5) < DateTimeOffset.Now)
                .Include(r => r.Book)
                .ToListAsync();

            foreach(var reservation in expiredReservations)
            {
                var onHoldCopy = await _context.BooksCopies
                    .FirstOrDefaultAsync(bc => bc.BookId == reservation.BookId && bc.Status == BookCopyStatus.OnHold);
                if(onHoldCopy != null)
                {
                    onHoldCopy.Status = BookCopyStatus.Available;
                }
                reservation.Status = ReservationSatus.Expired;
            }
            await _context.SaveChangesAsync();
        }
    }
}
