using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace QuizApp.Models
{
    public partial class TblQuiz
    {
        public TblQuiz()
        {
            QuestionTexts = new HashSet<QuestionText>();
            Questions = new HashSet<Question>();
            UserAnswers = new HashSet<UserAnswer>();
        }

        public int QuizId { get; set; }
        [Required]
        public string QuizName { get; set; }
        [Required]
        public string Time { get; set; }
        public string DateCreated { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<QuestionText> QuestionTexts { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
