using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LaborDay.Models
{
    public class MainView
    {
        public SelectList bettorList;
        //public List<GolferStats> golferStatList;
        public List<Golfer> golferList;
        public Bettor selectedBettor { get; set; }
        public Golfer selectedGolfer { get; set; }
        public Bet betRecord { get; set; }
        public string bettorSelect { set; get; }
        public int Money { set; get; }
        public List<TempBet> TempBets;
		public List<WinGroup> WinList;
		public List<PlaceGroup> PlaceList;
		public List<ShowGroup> ShowList;
		public List<WinGroup> WinWinnersList;
        public List<PlaceGroup> WinPlacersList;
        public List<ShowGroup> WinShowersList;
		public List<PlaceGroup> PlacePlacersList;
		public List<ShowGroup> PlaceShowersList;
		public List<ShowGroup> ShowShowersList;
		public List<Bettor> ListOfBettors;
    }
}
