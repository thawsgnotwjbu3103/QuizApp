using System;
using System.Collections.Generic;

#nullable disable

namespace QuizApp.Models
{
    public partial class UserAnswer
    {
        public int UserAnswersId { get; set; }
        public int? UserId { get; set; }
        public int? QuizId { get; set; }
        public int? QuestionId { get; set; }
        public int? ChoiceId { get; set; }
        public bool? IsRight { get; set; }

        public virtual QuestionChoice Choice { get; set; }
        public virtual Question Question { get; set; }
        public virtual TblQuiz Quiz { get; set; }
        public virtual UserInfo User { get; set; }
    }
}
