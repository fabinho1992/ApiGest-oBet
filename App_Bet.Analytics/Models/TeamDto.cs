using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bet.Analytics.Models
{
    public class TeamDto
    {
        public int id { get; set; }

        public string? conference { get; set; }
        public string? division { get; set; }

        public string? city { get; set; }
        public string? name { get; set; }
        public string? full_name { get; set; }
        public string? abbreviation { get; set; }
    }
}
