using System;
using System.Collections.Generic;

#nullable disable

namespace QuizApp.Models
{
    public partial class UserAnswer
    {
        public UserAnswer()
        {
            Points = new HashSet<Point>();
        }

        public int UserAnswersId { get; set; }
        public int? UserId { get; set; }
        public int? QuizId { get; set; }
        public int? QuestionId { get; set; }
        public int? ChoiceId { get; set; }
        public bool? IsRight { get; set; }

        public virtual QuestionChoice Choice { get; set; }
        public virtual Question Question { get; set; }
        public virtual TblQuiz Quiz { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Point> Points { get; set; }
    }
}
