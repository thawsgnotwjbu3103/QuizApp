using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace QuizApp.Models
{
    public partial class Question
    {
        public Question()
        {
            QuestionChoices = new HashSet<QuestionChoice>();
            UserAnswers = new HashSet<UserAnswer>();
        }

        public int QuestionId { get; set; }
        public int? QuizId { get; set; }
        [Required]
        public string QuestionTitle { get; set; }
        public bool IsMultipleChoices { get; set; }

        public virtual TblQuiz Quiz { get; set; }
        public virtual ICollection<QuestionChoice> QuestionChoices { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
