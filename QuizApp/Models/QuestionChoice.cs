using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace QuizApp.Models
{
    public partial class QuestionChoice
    {
        public QuestionChoice()
        {
            UserAnswers = new HashSet<UserAnswer>();
        }

        public int ChoiceId { get; set; }
        public int QuestionId { get; set; }
        public int? QuizId { get; set; }
        public bool IsRight { get; set; }
        [Required]
        public string Choice { get; set; }

        public virtual Question Question { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
