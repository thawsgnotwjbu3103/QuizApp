using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Models
{
    [Keyless]
    public class Admin
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public Admin ()
        {

        }

        public Admin(IdentityUser user)
        {
            Username = user.UserName;
            Password = user.PasswordHash;
        }
    }
}
