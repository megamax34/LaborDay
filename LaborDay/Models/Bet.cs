using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LaborDay.Models
{
    public class Bet
    {
        public int ID { get; set; }
        public int GolferId { get; set; }
        public int BettorID { get; set; }
        public bool Win { get; set; }
        public bool Place { get; set; }
        public bool Show { get; set; }

        public Golfer Golfer { get; set; }
        public Bettor Bettor { get; set; }
    }
}
