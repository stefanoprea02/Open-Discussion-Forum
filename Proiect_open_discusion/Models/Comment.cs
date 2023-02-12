using System.ComponentModel.DataAnnotations;

namespace Proiect_open_discusion.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Content is required")]
        [StringLength(1000, ErrorMessage = "Comment must have less than 1000 characters")]
        [MinLength(5, ErrorMessage = "Comment must have over 5 characters")]
        public string Text { get; set; }
        public DateTime TimePosted { get; set; }
        public string? ReplyFor { get; set; }
        public string? UserName { get; set; }
        public string? UserId { get; set; }
        public int? SubjectId { get; set; }
        //FKs
        public virtual ApplicationUser? User { get; set; }
        public virtual Subject? Subject { get; set; }
    }
}
