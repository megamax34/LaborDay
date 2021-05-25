using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaborDay.Models
{
    public class Bettor
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public ICollection<Bet> Bets { get; set; }
    }
}
