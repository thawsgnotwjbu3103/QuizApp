using System;
using System.Collections.Generic;

#nullable disable

namespace QuizApp.Models
{
    public partial class UserAnswerText
    {
        public int UaTextId { get; set; }
        public int? QuestionTextId { get; set; }
        public int? UserId { get; set; }
        public string QuestionTextTitle { get; set; }
        public string Matches { get; set; }

        public virtual QuestionText QuestionText { get; set; }
        public virtual UserInfo User { get; set; }
    }
}
