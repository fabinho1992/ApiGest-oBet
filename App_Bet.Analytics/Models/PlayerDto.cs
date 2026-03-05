using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_Bet.Analytics.Models
{
    public class PlayerDto
    {
        public int id { get; set; }

        public string? first_name { get; set; }
        public string? last_name { get; set; }

        public string? position { get; set; }
        public string? height { get; set; }
        public string? weight { get; set; }
        public string? jersey_number { get; set; }

        public string? college { get; set; }
        public string? country { get; set; }

        public int? draft_year { get; set; }
        public int? draft_round { get; set; }
        public int? draft_number { get; set; }

        public TeamDto? team { get; set; }
    }
}
