using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LaborDay.Data
{
    public class LaborDayContext : DbContext
    {
        public LaborDayContext (DbContextOptions<LaborDayContext> options)
            : base(options)
        {
        }

        public DbSet<LaborDay.Models.TempBet> TempBet { get; set; }
        public DbSet<LaborDay.Models.Bet> Bet { get; set; }
        public DbSet<LaborDay.Models.Bettor> Bettor { get; set; }
        public DbSet<LaborDay.Models.Golfer> Golfer { get; set; }
        public DbSet<LaborDay.Models.Scoreboard> Scoreboard { get; set; }
    }
}
