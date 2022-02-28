using System;
using System.Collections.Generic;

#nullable disable

namespace QuizApp.Models
{
    public partial class DisableList
    {
        public int Id { get; set; }
        public int DisableId { get; set; }
        public int UserId { get; set; }
    }
}
