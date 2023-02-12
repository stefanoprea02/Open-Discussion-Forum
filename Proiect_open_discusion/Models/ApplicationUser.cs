using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Proiect_open_discusion.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Subject>? Subjects { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
    }
}
