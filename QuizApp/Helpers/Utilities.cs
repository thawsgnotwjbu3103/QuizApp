using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApp.Helpers
{
    public static class Utilities
    {
        public static SelectList GenderList()
        {
            return (new SelectList(new List<SelectListItem> {
                new SelectListItem{ Text = "Nam" , Value = "Nam"},
                new SelectListItem{ Text = "Nữ" , Value = "Nữ"} }, "Value", "Text"));
        }
    }
}
