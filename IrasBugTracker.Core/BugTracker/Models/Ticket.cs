using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
    public enum Priority
    {
        Unknown = 0,
        Low = 1,
        Medium = 2,
        High = 3,
    }

    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Title must be between {2} and {1} characters long.", MinimumLength = 3)]
        public string Title { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        [Display(Name = "Ticket Priority")]
        public Priority TicketPriority { get; set; }

        // TODO: Use Enums
        // public int TicketStatusId { get; set; }
        // public int TicketTypeId { get; set; }

        // FKs
        [Required]
        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        public string OwnerId { get; set; } = String.Empty;
        public ApplicationUser? Owner { get; set; }

        public string? AssignedToId { get; set; }
        public ApplicationUser? AssignedTo { get; set; }
    }
}
