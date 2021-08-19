using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wall.Models
{
    public class Comment
    {
        public int CommentID { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Comment must have at least one character.")]
        [Display(Name ="Post a comment")]
        public string AComment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public int UserID { get; set; }
        public User User { get; set; }
        public int MessageID { get; set; }
        public Message Message { get; set; } 
    }
}