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
    public class LoanController : ControllerBase
    {
        private readonly BookLibraryDbContext _context;
        public LoanController(BookLibraryDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<LoanDto>>> GetLoand()
        {
            var loan = await _context.Loans
                .Include(b => b.BookCopy)
                    .ThenInclude(bc => bc!.Book)
                .Include(l => l.Member)
                .Select(b => new LoanDto
                {
                   Id = b.Id,
                   BookCopyId = b.BookCopyId,
                   BookTitle = b.BookCopy.Book.Title,
                   CopyNumber = b.BookCopy.CopyNumber,
                   Borrower = b.Member!.FullName,
                   MemberId = b.MemberId,
                   LoanDate = b.LoanDate,
                   DueDate = b.DueDate,
                   ReturnDate = b.ReturnDate

                }).ToListAsync();
            return Ok(loan);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<LoanDto>> GetLoanById(int id)
        {
            var existingLoan = await _context.Loans
                .Where(b => b.Id == id)
                .Include(b => b.BookCopy)
                .ThenInclude(bc => bc!.Book)
                .Include(l => l.Member)
                .Select(b => new LoanDto
                {
                    Id = b.Id,
                    BookCopyId = b.BookCopyId,
                    MemberId= b.MemberId,
                    Borrower = b.Member!.FullName,
                    BookTitle = b.BookCopy.Book.Title,
                    LoanDate= b.LoanDate,
                    DueDate= b.DueDate,
                    ReturnDate = b.ReturnDate

                }).FirstOrDefaultAsync();
            if(existingLoan == null)
            {
                return NotFound();
            }
            return Ok(existingLoan);
        }
        [HttpPost]
        public async Task<ActionResult> AddLoan(CreateLoanDto newLoanDto)
        {
            if (newLoanDto == null)
                return BadRequest();
            var bookCopy = await _context.BooksCopies.FindAsync(newLoanDto.BookCopyId);
            if(bookCopy == null)
                return  NotFound("Book copy is not found");
            if(bookCopy.Status != BookCopyStatus.Available)
                return BadRequest("Book copy is already borrowed");
            var loan = new Loan
            {
                MemberId = newLoanDto.MemberId,
                BookCopyId = newLoanDto.BookCopyId,
                LoanDate = DateTimeOffset.UtcNow,
                DueDate= newLoanDto.DueDate
            };
            bookCopy.Status = BookCopyStatus.Loaned;

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();
            //returning the create loan to the dto
            var loanDto = new LoanDto
            {
                Id = loan.Id,
                BookCopyId = loan.BookCopyId,
                MemberId = loan.MemberId,
                LoanDate = loan.LoanDate,
                DueDate = loan.DueDate
            };
            return CreatedAtAction(nameof(GetLoanById), new {id = loan.Id}, loanDto);
           
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLoan(int id, CreateLoanDto updatedLoandDto)
        {
            var existingLoan = await _context.Loans.FindAsync(id);
            if (existingLoan == null)
                return NotFound();
            existingLoan.MemberId = updatedLoandDto.MemberId;
            existingLoan.BookCopyId = updatedLoandDto.BookCopyId;
            existingLoan.DueDate = updatedLoandDto.DueDate;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(int id)
        {
            var existingLoan = await _context.Loans.FindAsync();
            if(existingLoan == null)
                return NotFound();
            _context.Loans.Remove(existingLoan);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("return")]
        public async Task<IActionResult> ReturnBook([FromBody] ReturnBookDto returnDto)
        {
            var loan = await _context.Loans
                .Include(l => l.BookCopy)
                .FirstOrDefaultAsync( l => l.Id == returnDto.LoanId);
            if (loan == null) 
                return NotFound("Loan record not found");
            if (loan.ReturnDate != null)
                return BadRequest("Book has already been returned.");
            if (loan.BookCopy == null)
                return NotFound("Associated book copy not found");
            loan.ReturnDate = DateTimeOffset.Now;
            loan.BookCopy.Status = BookCopyStatus.Available;

            //including the book so as to get the Id.
            await _context.BooksCopies.Include(bc => bc.Book).LoadAsync();
            var bookId = loan.BookCopy.Book.Id;

            var pendingReservation = await _context.Reservations
                .Where(r => r.BookId == bookId && r.Status == ReservationSatus.Pending)
                .OrderBy(r => r.ReservationDate)
                .FirstOrDefaultAsync();
            if(pendingReservation != null)
            {
                loan.BookCopy.Status = BookCopyStatus.OnHold;
                pendingReservation.Status = ReservationSatus.ReadyForPickup;
            }
            else
            {
                loan.BookCopy.Status = BookCopyStatus.Available;
            }

                await _context.SaveChangesAsync();
            return Ok(new {message = "Book has been successfully reuturned", loanId = loan.Id});
        }
        [HttpPut("{id}/renewLoan")]
        public async Task<IActionResult> RenewLoan(int id, RenewLoanDto renewDto)
        {
            var loan = await _context.Loans.FindAsync(id);
                
            if (loan == null)
                return NotFound("No loan is found");
            if (loan.ReturnDate != null)
                return BadRequest("Cannot extend duedate for a book already returned");

            if (loan.DueDate < DateTimeOffset.Now)
                return BadRequest("Due date has been exceeded! Cannot extend the due date again");
            loan.DueDate = renewDto.DueDate;
            loan.Renewals++;
            if (loan.Renewals > 2)

                return BadRequest("reached renewal limit");
            await _context.SaveChangesAsync();
            return Ok(new { message = "Successfully extended due date", loanId = id });
        }

        [HttpPost("checkout/{reservationId}")]
        public async Task<IActionResult> CheckoutFromHold(int reservationId)
        {
            var reservation = await _context.Reservations
                .Include(l => l.Member)
                .FirstOrDefaultAsync(r => r.Id == reservationId);
            if (reservation == null)
                return NotFound("Reservation not found");
            if (reservation.Status != ReservationSatus.ReadyForPickup)
                return BadRequest("This reservation is not ready for pickup");
            
            //finding an available book copy that is on hold for this book title
            var bookCopy = await _context.BooksCopies
                .Include(b =>b.Book)
                .FirstOrDefaultAsync(bc => bc.BookId == reservation.BookId && bc.Status == BookCopyStatus.OnHold);
            if (bookCopy == null)
                return NotFound("No book Copy is currently on hold for this reservation");
            var newLoan = new Loan
            {
                MemberId = reservation.MemberId,
                BookCopyId = bookCopy.Id,
                LoanDate = DateTimeOffset.Now,
                DueDate = DateTime.Now.AddDays(10)
            };
            bookCopy.Status = BookCopyStatus.Loaned;
            reservation.Status = ReservationSatus.Completed;

            _context.Loans.Add(newLoan);
            await _context.SaveChangesAsync();

            var loanDto = new LoanDto
            {
                Id = newLoan.Id,
                BookCopyId = newLoan.BookCopyId,
                MemberId = newLoan.MemberId,
                BookTitle = bookCopy.Book.Title,
                Borrower = reservation.Member!.FullName,
                LoanDate = newLoan.LoanDate,
                DueDate = newLoan.DueDate
            };

            return Ok(new {message = "Book successfully checked out from hold.", loan = loanDto});

        }
    }
}
