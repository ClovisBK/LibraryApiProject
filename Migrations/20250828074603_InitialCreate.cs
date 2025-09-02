using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibrarySystemApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JoinedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    PublicationYear = table.Column<int>(type: "int", nullable: false),
                    Isbn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pages = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BooksCopies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    CopyNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BooksCopies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BooksCopies_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookCopyId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    LoanDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loans_BooksCopies_BookCopyId",
                        column: x => x.BookCopyId,
                        principalTable: "BooksCopies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Loans_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Country", "Name" },
                values: new object[,]
                {
                    { 1, "USA", "Ryan Holiday" },
                    { 2, "Rome", "Marcus Aurelius" },
                    { 3, "Rome", "Seneca" },
                    { 4, "Turkey", "Epictetus" },
                    { 5, "USA", "Tim Ferriss" },
                    { 6, "USA", "Robert Greene" },
                    { 7, "USA", "James Clear" },
                    { 8, "USA", "Dale Carnegie" },
                    { 9, "USA", "Stephen Covey" },
                    { 10, "USA", "Cal Newport" }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Philosophy" },
                    { 2, "Stoicism" },
                    { 3, "Self-help" },
                    { 4, "Productivity" },
                    { 5, "Biography" },
                    { 6, "History" },
                    { 7, "Science" },
                    { 8, "Psychology" },
                    { 9, "Business" },
                    { 10, "Non-fiction" }
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "Address", "Email", "FullName", "JoinedDate", "Phone" },
                values: new object[,]
                {
                    { 1, "Mvog-Betsi, Baobab", "kebehclovis@gmail.com", "Kebeh Clovis", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+237 679695180" },
                    { 2, "Ohio, Bellevue", "amira.johnson@example.com", "Amira Johnson", new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "+1 434534534455" },
                    { 3, "Obili", "david.lee@example.com", "David Lee", new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "+237 654432443" },
                    { 4, "Nkolbisson", "fatima.ali@example.com", "Fatima Ali", new DateTime(2025, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "+237 654345676" },
                    { 5, "Leke steet 1", "luca.rossi@example.com", "Luca Rossi", new DateTime(2022, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "+234 5334645345" },
                    { 6, "", "sophia.mueller@example.com", "Sophia Müller", new DateTime(2023, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "+237 674534321" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "GenreId", "ImageUrl", "Isbn", "Pages", "PublicationYear", "Title" },
                values: new object[,]
                {
                    { 1, 1, 2, "obstacle-the-way.jpg", "9781591846352", 224, 2014, "The Obstacle Is the Way" },
                    { 2, 2, 2, "meditations.jpg", "9780140449334", 304, 180, "Meditations" },
                    { 3, 3, 2, "letters-from-stoic.jpg", "9780140442106", 256, 65, "Letters from a Stoic" },
                    { 4, 4, 2, "art-of-living.jpg", "9780061286056", 144, 55, "The Art of Living" },
                    { 5, 5, 4, "tools-of-titans.jpg", "9781328683786", 704, 2016, "Tools of Titans" },
                    { 6, 6, 3, "laws-of-power.jpg", "9780140280197", 452, 1998, "The 48 Laws of Power" },
                    { 7, 7, 3, "atomic-habits.jpg", "9780735211292", 320, 2018, "Atomic Habits" },
                    { 8, 8, 3, "win-friends-and-people.jpg", "9780671027032", 291, 1936, "How to Win Friends and Influence People" },
                    { 9, 9, 3, "effective-people.jpg", "9780743269513", 381, 1989, "The 7 Habits of Highly Effective People" },
                    { 10, 10, 4, "deep-work.jpg", "9781455586691", 304, 2016, "Deep Work" },
                    { 11, 1, 3, "ego-is-enemyjpg", "9781591847816", 256, 2016, "Ego Is the Enemy" },
                    { 12, 1, 3, "stillness-is-key.jpg", "9780525538585", 288, 2019, "Stillness Is the Key" },
                    { 13, 1, 2, "daily-stoic.jpg", "9780735211735", 416, 2016, "Daily Stoic" },
                    { 14, 6, 3, "laws-of-human-nature.jpg", "9780525428145", 624, 2018, "The Laws of Human Nature" },
                    { 15, 5, 4, "tribe-of-mentors.jpg", "9781328994967", 624, 2017, "Tribe of Mentors" }
                });

            migrationBuilder.InsertData(
                table: "BooksCopies",
                columns: new[] { "Id", "Barcode", "BookId", "CopyNumber", "Status" },
                values: new object[,]
                {
                    { 1, "123456789001", 1, "copy #1", "Available" },
                    { 2, "123456734401", 1, "copy #2", "Available" },
                    { 3, "12355559001", 1, "copy #3", "Available" },
                    { 4, "123456789222", 3, "copy #1", "Available" },
                    { 5, "1234567891111", 1, "copy #4", "Available" }
                });

            migrationBuilder.InsertData(
                table: "Loans",
                columns: new[] { "Id", "BookCopyId", "DueDate", "LoanDate", "MemberId", "ReturnDate" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null },
                    { 2, 2, new DateTime(2024, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, null },
                    { 3, 3, new DateTime(2024, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorId",
                table: "Books",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_GenreId",
                table: "Books",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_BooksCopies_BookId_Barcode",
                table: "BooksCopies",
                columns: new[] { "BookId", "Barcode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BooksCopies_BookId_CopyNumber",
                table: "BooksCopies",
                columns: new[] { "BookId", "CopyNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_BookCopyId",
                table: "Loans",
                column: "BookCopyId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_MemberId",
                table: "Loans",
                column: "MemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "BooksCopies");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Genres");
        }
    }
}
