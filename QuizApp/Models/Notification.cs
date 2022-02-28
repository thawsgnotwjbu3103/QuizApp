using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace QuizApp.Models
{
    public partial class Notification
    {
        public int NotifyId { get; set; }
        public string Title { get; set; }
        public string NotifyContent { get; set; }
        public string DateCreated { get; set; }
        public string DateUpdated { get; set; }
        public bool IsActive { get; set; }

        [NotMapped]
        public IFormFile Content { get; set; }

    }
}
