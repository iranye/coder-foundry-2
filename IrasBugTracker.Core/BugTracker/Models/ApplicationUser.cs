using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First Name must be between {2} and {1} characters long.", MinimumLength = 2)]
        public string FirstName { get; set; } = String.Empty;

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last Name must be between {2} and {1} characters long.", MinimumLength = 2)]
        public string LastName { get; set; } = String.Empty;

        [Display(Name = "Display Name")]
        [StringLength(90, ErrorMessage = "Display Name must be no longer than {1} characters long.")]
        public string DisplayName { get; set; } = String.Empty;

        public bool IsDemoUser { get; set; }

        public ICollection<Project> Projects { get; set; }
        public ICollection<TicketComment> TicketComments { get; set; }
        public ICollection<TicketHistory> TicketHistories { get; set; }

        public ApplicationUser()
        {
            Projects = new HashSet<Project>();
            TicketComments = new HashSet<TicketComment>();
            TicketHistories = new HashSet<TicketHistory>();
        }
    }
}