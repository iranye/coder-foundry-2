namespace BugTracker.Models
{
    public class TicketType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navs
        public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
