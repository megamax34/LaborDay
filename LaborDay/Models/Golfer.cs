using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaborDay.Models
{
    public class Golfer
    {
        public int ID { get; set; }
        public string GolferName { get; set; }
        public bool Playing { get; set; }

        public ICollection<Bet> Bets { get; set; }

    }
}
