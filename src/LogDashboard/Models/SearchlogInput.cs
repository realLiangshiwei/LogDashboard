using System;

namespace LogDashboard.Models
{
    public class SearchLogInput
    {
        public bool All { get; set; }

        public bool Unique { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool ToDay { get; set; }

        public LogLevel? Level { get; set; }

        public bool Hour { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public string Message { get; set; }

        public SearchLogInput()
        {
            Page = 1;
            PageSize = 10;
        }
    }
}
