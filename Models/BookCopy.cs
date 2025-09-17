using System.ComponentModel;

namespace LibrarySystemApi.Models
{
    public class BookCopy
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public string CopyNumber { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public BookCopyStatus Status { get; set; }

    }
    public enum BookCopyStatus
    {
        Available,
        Loaned,
        Reserved,
        Lost,
        Damaged,
        [Description("On Hold")]
        OnHold,
    }
}
