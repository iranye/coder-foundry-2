namespace BugTracker.Models
{
    public class TicketComment
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string CommentBody { get; set; }

        // FKs
        public int TicketId { get; set; }
        public string AuthorId { get; set; }

        // Navs
        public virtual Ticket Ticket { get; set; }
    }
}
