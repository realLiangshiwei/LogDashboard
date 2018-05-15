using System;
using System.Collections.Generic;
using System.Text;

namespace NlogDashboard.Model
{
    public class SearchlogInput
    {
        public bool All { get; set; }

        public bool Unique { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool ToDay { get; set; }

        public string Level { get; set; }

        public bool Hour { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public SearchlogInput()
        {
            Page = 1;
            PageSize = 20;
        }
    }
}
