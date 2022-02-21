using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace QuizApp.Models
{
    public partial class User
    {
        public User()
        {
            UserAnswers = new HashSet<UserAnswer>();
        }

        public int UserId { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Birthday { get; set; }
        [Required]
        public string IdNum { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PhoneNum { get; set; }
        public string Email { get; set; }
        public string DateCreated { get; set; }

        public virtual Point Point { get; set; }
        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
