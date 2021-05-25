using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaborDay.Models
{
    public class Scoreboard
    {
        public int ID { get; set; }
        public string GolferName { get; set; }
        public int Hole { get; set; }
        public int Score { get; set; }
    }
}
