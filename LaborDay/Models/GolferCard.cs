using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaborDay.Models
{
    public class GolferCard
    {
        public string GolferName { get; set; }
        public List<int> ScoreList { get; set; }
        public List<int> HoleList { get; set; }
        public int Total { get; set; }
        
    }
}
