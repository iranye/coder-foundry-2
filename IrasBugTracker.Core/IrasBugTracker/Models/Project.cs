using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        // Navs
        public virtual ICollection<Ticket> Tickets { get; set; }

        public Project()
        {
            Tickets = new HashSet<Ticket>();
        }
    }
}
