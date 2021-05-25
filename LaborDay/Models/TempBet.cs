using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaborDay.Models
{
    public class TempBet
    {
       
        public int ID { get; set; }
        public int GolferId { get; set; }
        public int BettorID { get; set; }
        public bool Win { get; set; }
        public bool Place { get; set; }
        public bool Show { get; set; }
        public int Money { get; set; }
        
    }
}
