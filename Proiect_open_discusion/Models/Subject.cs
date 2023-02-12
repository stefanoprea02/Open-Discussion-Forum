using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proiect_open_discusion.Models
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title must have less than 100 characters")]
        [MinLength(5, ErrorMessage = "Title must be over 5 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Content is required")]
        [StringLength(1000, ErrorMessage = "Content must have less than 1000 characters")]
        [MinLength(5, ErrorMessage = "Content must be over 5 characters")]
        public string Text { get; set; }
        public DateTime TimePosted { get; set; }
        [Required(ErrorMessage = "Category is required")]
        public int? CategoryId { get; set; }
        public string? UserId { get; set; }
        //FKs
        public virtual ApplicationUser? User { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual Category? Category { get; set; }
        [NotMapped]
        public IEnumerable<SelectListItem>? Categ { get; set; }
    }
}
