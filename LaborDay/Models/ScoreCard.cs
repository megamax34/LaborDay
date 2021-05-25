using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaborDay.Models
{
    public class ScoreCard
    {
        public SelectList Golfers { get; set; }
        public SelectList Holes { get; set; }
        public SelectList Scores { get; set; }

        public string golferSelect { set; get; }
        public int holeSelect { set; get; }
        public int scoreSelect { set; get; }
    }
}
