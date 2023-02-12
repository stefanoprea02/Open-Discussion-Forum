using System.ComponentModel.DataAnnotations;

namespace Proiect_open_discusion.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(40, ErrorMessage = "Name must have less than 40 characters")]
        [MinLength(4, ErrorMessage = "Name must have over 4 characters")]
        public string Name { get; set; }
        //FKs
        public virtual ICollection<Subject>? Subjects { get; set; } = new List<Subject>();
    }
}
