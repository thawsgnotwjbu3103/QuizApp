using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace QuizApp.Models
{
    public partial class TblQuiz
    {
        public TblQuiz()
        {
            Questions = new HashSet<Question>();
            QuestionsTexts = new HashSet<QuestionsText>();
            UserAnswers = new HashSet<UserAnswer>();
        }

        public int QuizId { get; set; }
        [Required]
        public string QuizName { get; set; }
        [Required]
        public string Time { get; set; }
        [Required]
        public string DateCreated { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<QuestionsText> QuestionsTexts { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
