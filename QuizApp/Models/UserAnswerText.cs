using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace QuizApp.Models
{
    public partial class UserAnswerText
    {
        public UserAnswerText()
        {
            Points = new HashSet<Point>();
        }

        public int UaTextId { get; set; }
        public int? QuestionTextId { get; set; }
        public int? UserId { get; set; }
        public string QuestionTextTitle { get; set; }
        [Required]
        public string Matches { get; set; }

        public virtual QuestionText QuestionText { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Point> Points { get; set; }
    }
}
