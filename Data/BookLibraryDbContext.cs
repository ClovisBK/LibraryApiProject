using LibrarySystemApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystemApi.Data
{
    public class BookLibraryDbContext(DbContextOptions <BookLibraryDbContext> options): DbContext(options)
    {
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<Member> Members => Set<Member>();
        public DbSet<Loan> Loans => Set<Loan>();
        public DbSet<BookCopy> BooksCopies => Set<BookCopy>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Admin> Admins => Set<Admin>();



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookCopy>()
                .Property(b => b.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Reservation>()
                .Property(r => r.Status)
                .HasConversion<string>();

            modelBuilder.Entity<BookCopy>()
                .HasIndex(b => new { b.BookId, b.CopyNumber })
                .IsUnique();

            modelBuilder.Entity<BookCopy>()
               .HasIndex(b => new { b.BookId, b.Barcode })
               .IsUnique();


            modelBuilder.Entity<Genre>().HasData(
                 new Genre { Id = 1, Name = "Philosophy" },
                 new Genre { Id = 2, Name = "Stoicism" },
                 new Genre { Id = 3, Name = "Self-help" },
                 new Genre { Id = 4, Name = "Productivity" },
                 new Genre { Id = 5, Name = "Biography" },
                 new Genre { Id = 6, Name = "History" },
                 new Genre { Id = 7, Name = "Science" },
                 new Genre { Id = 8, Name = "Psychology" },
                 new Genre { Id = 9, Name = "Business" },
                 new Genre { Id = 10, Name = "Non-fiction" }
             );

             //modelBuilder.Entity<Member>().HasData(
             //   new Member {
             //       Id = 1, FullName = "Kebeh Clovis",
             //       Email = "kebehclovis@gmail.com",
             //       JoinedDate = new DateTime(2024, 01, 01),
             //       Phone = "+237 679695180",
             //       Address = "Mvog-Betsi, Baobab"
             //   },
             //    new Member {
             //        Id = 2, FullName = "Amira Johnson", 
             //        Email = "amira.johnson@example.com", 
             //        JoinedDate = new DateTime(2024, 02, 15),
             //        Phone = "+1 434534534455",
             //        Address = "Ohio, Bellevue"
             //    },
             //     new Member { Id = 3, FullName = "David Lee",
             //         Email = "david.lee@example.com",
             //         JoinedDate = new DateTime(2024, 03, 10),
             //         Phone = "+237 654432443",
             //         Address = "Obili"
             //     },
             //    new Member {
             //        Id = 4, FullName = "Fatima Ali",
             //        Email = "fatima.ali@example.com", 
             //        JoinedDate = new DateTime(2025, 02, 12),
             //        Phone = "+237 654345676",
             //        Address = "Nkolbisson"
             //    },
             //    new Member {
             //        Id = 5, FullName = "Luca Rossi",
             //        Email = "luca.rossi@example.com", 
             //        JoinedDate = new DateTime(2022, 10, 10),
             //        Phone = "+234 5334645345",
             //        Address = "Leke steet 1"
             //    },
             //    new Member {
             //        Id = 6, FullName = "Sophia Müller", 
             //        Email = "sophia.mueller@example.com",
             //        JoinedDate = new DateTime(2023, 11, 16),
             //        Phone = "+237 674534321"
             //    }
             //);

             //           modelBuilder.Entity<Loan>().HasData(
             //      new Loan
             //      {
             //          Id = 1,
             //          BookCopyId = 1, 
             //          MemberId = 1, 
             //          LoanDate = new DateTime(2024, 6, 1),
             //          DueDate = new DateTime(2024, 6, 15)
             //      },
             //      new Loan
             //      {
             //          Id = 2,
             //          BookCopyId = 2,
             //          MemberId = 2,
             //          LoanDate = new DateTime(2024, 6, 5),
             //          DueDate = new DateTime(2024, 6, 20)
             //      },
             //      new Loan
             //      {
             //          Id = 3,
             //          BookCopyId = 3,
             //          MemberId = 3,
             //          LoanDate = new DateTime(2024, 6, 10),
             //          DueDate = new DateTime(2024, 6, 25)
             //      }
             //  );

            // Seed Authors
            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "Ryan Holiday", Country = "USA" },
                new Author { Id = 2, Name = "Marcus Aurelius", Country = "Rome" },
                new Author { Id = 3, Name = "Seneca", Country = "Rome" },
                new Author { Id = 4, Name = "Epictetus", Country = "Turkey" },
                new Author { Id = 5, Name = "Tim Ferriss", Country = "USA" },
                new Author { Id = 6, Name = "Robert Greene", Country = "USA" },
                new Author { Id = 7, Name = "James Clear", Country = "USA" },
                new Author { Id = 8, Name = "Dale Carnegie", Country = "USA" },
                new Author { Id = 9, Name = "Stephen Covey", Country = "USA" },
                new Author { Id = 10, Name = "Cal Newport", Country = "USA" }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "The Obstacle Is the Way", ImageUrl = "obstacle-the-way.jpg", PublicationYear = 2014, Isbn = "9781591846352", Pages = 224, AuthorId = 1, GenreId = 2 },
                new Book { Id = 2, Title = "Meditations", ImageUrl = "meditations.jpg", PublicationYear = 180, Isbn = "9780140449334", Pages = 304, AuthorId = 2, GenreId = 2 },
                new Book { Id = 3, Title = "Letters from a Stoic",ImageUrl="letters-from-stoic.jpg", PublicationYear = 65, Isbn = "9780140442106", Pages = 256, AuthorId = 3, GenreId = 2 },
                new Book { Id = 4, Title = "The Art of Living",ImageUrl="art-of-living.jpg", PublicationYear = 55, Isbn = "9780061286056", Pages = 144, AuthorId = 4, GenreId = 2 },
                new Book { Id = 5, Title = "Tools of Titans",ImageUrl="tools-of-titans.jpg", PublicationYear = 2016, Isbn = "9781328683786", Pages = 704, AuthorId = 5, GenreId = 4 },
                new Book { Id = 6, Title = "The 48 Laws of Power",ImageUrl="laws-of-power.jpg", PublicationYear = 1998, Isbn = "9780140280197", Pages = 452, AuthorId = 6, GenreId = 3 },
                new Book { Id = 7, Title = "Atomic Habits",ImageUrl ="atomic-habits.jpg", PublicationYear = 2018, Isbn = "9780735211292", Pages = 320, AuthorId = 7, GenreId = 3 },
                new Book { Id = 8, Title = "How to Win Friends and Influence People",ImageUrl="win-friends-and-people.jpg", PublicationYear = 1936, Isbn = "9780671027032", Pages = 291, AuthorId = 8, GenreId = 3 },
                new Book { Id = 9, Title = "The 7 Habits of Highly Effective People",ImageUrl="effective-people.jpg", PublicationYear = 1989, Isbn = "9780743269513", Pages = 381, AuthorId = 9, GenreId = 3 },
                new Book { Id = 10, Title = "Deep Work",ImageUrl="deep-work.jpg", PublicationYear = 2016, Isbn = "9781455586691", Pages = 304, AuthorId = 10, GenreId = 4 },
                new Book { Id = 11, Title = "Ego Is the Enemy",ImageUrl="ego-is-enemyjpg", PublicationYear = 2016, Isbn = "9781591847816", Pages = 256, AuthorId = 1, GenreId = 3 },
                new Book { Id = 12, Title = "Stillness Is the Key",ImageUrl="stillness-is-key.jpg", PublicationYear = 2019, Isbn = "9780525538585", Pages = 288, AuthorId = 1, GenreId = 3 },
                new Book { Id = 13, Title = "Daily Stoic",ImageUrl="daily-stoic.jpg", PublicationYear = 2016, Isbn = "9780735211735", Pages = 416, AuthorId = 1, GenreId = 2 },
                new Book { Id = 14, Title = "The Laws of Human Nature",ImageUrl="laws-of-human-nature.jpg", PublicationYear = 2018, Isbn = "9780525428145", Pages = 624, AuthorId = 6, GenreId = 3 },
                new Book { Id = 15, Title = "Tribe of Mentors",ImageUrl="tribe-of-mentors.jpg", PublicationYear = 2017, Isbn = "9781328994967", Pages = 624, AuthorId = 5, GenreId = 4 }
            );

            modelBuilder.Entity<BookCopy>().HasData(
                    new BookCopy
                    {
                        Id = 1,
                        BookId = 1,
                        Barcode = "123456789001",
                        CopyNumber = "copy #1",
                        Status = Enum.Parse<BookCopyStatus>("Available")
                    },
                    new BookCopy
                    {
                        Id = 2,
                        BookId = 1,
                        Barcode = "123456734401",
                        CopyNumber = "copy #2",
                        Status = Enum.Parse<BookCopyStatus>("Available")
                    },
                    new BookCopy
                    {
                        Id = 3,
                        BookId = 1,
                        Barcode = "12355559001",
                        CopyNumber = "copy #3",
                        Status = Enum.Parse<BookCopyStatus>("Available")
                    },
                    new BookCopy
                    {
                        Id = 4,
                        BookId = 3,
                        Barcode = "123456789222",
                        CopyNumber = "copy #1",
                        Status = Enum.Parse<BookCopyStatus>("Available")
                    },
                    new BookCopy
                    {
                        Id = 5,
                        BookId = 1,
                        Barcode = "1234567891111",
                        CopyNumber = "copy #4",
                        Status = Enum.Parse<BookCopyStatus>("Available")
                    }
                );

            var adminId = new Guid("11111111-1111-1111-1111-111111111111");
            var hash = new PasswordHasher<User>().HashPassword(null!, "password!23");
            Console.WriteLine(hash);
            var userEmail = "clovis@mail.com";
            var adminUser = new User
            {
                Id = adminId,
                Email = userEmail,
                Role = UserRoles.Admin.ToString(),
                PasswordHash = "AQAAAAIAAYagAAAAEFK0oYnrWfEwye5H/s6oxNRwVBLxbSqrj6X0/dPwVg5vRsQom3Vu/gNF2x6R9uKWQQ=="
            };
            
            modelBuilder.Entity<User>().HasData(adminUser);

            var adminProfile = new Admin
            {
                Id = 1,
                FullName = "Clovis Bin Kebeh",
                UserId = adminId,

                Email = userEmail,
                Phone = "+237645645432",
                Address = "Mvog-Betsi",
                JoinedDate = new DateTime(2025, 09, 15)

            };
            modelBuilder.Entity<Admin>().HasData(adminProfile);
           
        }
    }
}
