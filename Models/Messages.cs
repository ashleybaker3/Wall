using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wall.Models
{
    public class Message
    {
        public int MessageID { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Message must have at least one character.")]
        [Display(Name = "Post a Message")]
        public string AMessage { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int UserID { get; set; }
        public User Creator { get; set; }
        public List<Comment> UserMesComments { get; set; }

    }
}