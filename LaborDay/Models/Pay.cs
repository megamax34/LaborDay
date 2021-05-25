using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace LaborDay.Models
{
    public class Pay
    {
        public SelectList Golfers;
        public string GolferWin { get; set; }
        public string GolferPlace { get; set; }
        public string GolferShow { get; set; }
    }
}
