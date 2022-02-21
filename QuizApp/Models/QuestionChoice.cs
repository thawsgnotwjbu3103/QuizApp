using System;
using System.Collections.Generic;

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
        public bool IsRight { get; set; }
        public string Choice { get; set; }

        public virtual Question Question { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
