using System;
using System.Collections.Generic;

#nullable disable

namespace QuizApp.Models
{
    public partial class Point
    {
        public int PointId { get; set; }
        public int? QuizId { get; set; }
        public int UserId { get; set; }
        public int TotalPoint { get; set; }
    }
}
