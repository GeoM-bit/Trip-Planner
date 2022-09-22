using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TripPlanner.DatabaseModels.Models
{
    public class User : IdentityUser<Guid>
    {
        [Required, MaxLength(50)]
        public virtual string FirstName { get; set; }

        [Required, MaxLength(50)]
        public virtual string LastName { get; set; }
    }
}
