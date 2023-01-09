namespace BugTracker.Models
{
    public class TicketComment
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public String CommentBody { get; set; } = String.Empty;

        // FKs
        public int TicketId { get; set; }
        public String AuthorId { get; set; } = String.Empty;

        // Navs
        public Ticket? Ticket { get; set; }
    }
}
