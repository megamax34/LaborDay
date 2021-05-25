using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaborDay.Models;

namespace LaborDay.Data
{
    public static class SeedTest
    {
        public static void Initialize(LaborDayContext context)
        {
            context.Database.EnsureCreated();

            if (context.TempBet.Count() > 0)
            {
                foreach (var b in context.TempBet)
                {
                    context.TempBet.Remove(b);
                }
            }
            if (context.Bet.Any() || context.Golfer.Any())
            {
                return;   // DB has been seeded
            }

            var bettors = new Bettor[]
            {
                new Bettor{Name="Ann"}
            };
            foreach (Bettor b in bettors)
            {
                context.Bettor.Add(b);
            }
            context.SaveChanges();

            var golfers = new Golfer[]
            {
                new Golfer{GolferName="James", Playing=true},
                new Golfer{GolferName="Ted", Playing=false}
            };
            foreach (Golfer g in golfers)
            {
                context.Golfer.Add(g);
            }
            context.SaveChanges();
/*
            var score = new Scoreboard[] {}

            foreach (var g in context.Golfer)
            {
                for (int i = 1; i<=18; i++)
                {

                    score.Add(new Scoreboard { GolferName = g.GolferName, Hole = i, Score = 0 });
                    
                }
               
            }
*/
            /*
            var enrollments = new Enrollment[]
            {
            
            };
            foreach (Enrollment e in enrollments)
            {
                context.Enrollment.Add(e);
            }
            context.SaveChanges();
            */
        }
    }
}
