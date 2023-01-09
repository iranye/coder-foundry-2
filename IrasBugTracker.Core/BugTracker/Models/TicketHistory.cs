namespace BugTracker.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }

        public string Property { get; set; } = String.Empty;
        public string OldValue { get; set; } = String.Empty;
        public string NewValue { get; set; } = String.Empty;
        public DateTime ChangedDateTime { get; set; }

        // FKs
        public int TicketId { get; set; }
        public string ChangedById { get; set; } = String.Empty;

        // Navs
        public Ticket? Ticket { get; set; }
    }
}
