namespace BugTracker.Models
{
    public class TicketHistory
    {
        public int Id { get; set; }

        public string Property { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime ChangedDateTime { get; set; }

        // FKs
        public int TicketId { get; set; }
        public string ChangedById { get; set; }

        // Navs
        public virtual Ticket Ticket { get; set; }
    }
}
