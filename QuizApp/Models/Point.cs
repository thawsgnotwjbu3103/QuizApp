using System;
using System.Collections.Generic;

#nullable disable

namespace QuizApp.Models
{
    public partial class Point
    {
        public int PointId { get; set; }
        public int UserId { get; set; }
        public int TotalPoint { get; set; }
        public int? UaTextId { get; set; }
        public int? UserAnswersId { get; set; }

        public virtual User PointNavigation { get; set; }
        public virtual UserAnswerText UaText { get; set; }
        public virtual UserAnswer UserAnswers { get; set; }
    }
}
