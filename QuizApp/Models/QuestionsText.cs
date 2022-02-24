using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace QuizApp.Models
{
    public partial class QuestionsText
    {
        public QuestionsText()
        {
            UserAnswerTexts = new HashSet<UserAnswerText>();
        }

        public int QuestionTextId { get; set; }
        [Required]
        public string QuestionTextTitle { get; set; }
        public int? QuizId { get; set; }

        public virtual TblQuiz Quiz { get; set; }
        public virtual ICollection<UserAnswerText> UserAnswerTexts { get; set; }
    }
}
