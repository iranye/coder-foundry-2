namespace BugTracker.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        public string MediaPath { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; }

        // FKs
        public int TicketId { get; set; }
        public string CreatedById { get; set; }

        // Navs
        public virtual Ticket Ticket { get; set; }
    }
}
