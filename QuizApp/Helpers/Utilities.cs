using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

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
