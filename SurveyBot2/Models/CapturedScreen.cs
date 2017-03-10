using System;
using System.Collections.Generic;

namespace SurveyBot2.Models
{
    public class CapturedScreen
    {
        public string ScreenId { get; set; }
        public string QuestionText { get; set; }
        public List<string> Options = new List<string>();
        public List<Category> Categories = new List<Category>();
        public Uri RequestUri { get; set; }
        public string HistoryOrder { get; set; }
    }
}