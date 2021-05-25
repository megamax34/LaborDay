using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaborDay.Data;
using LaborDay.Models;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
 

namespace LaborDay.Controllers
{
    public class BetsController : Controller
    {
        private readonly LaborDayContext _context;

        public BetsController(LaborDayContext context)
        {
            _context = context;
        }
		 
        public IQueryable<WinGroup> CalculateWins()
        {
            IQueryable<WinGroup> windata =
               from bet in _context.Bet
               where bet.Win == true
               group bet by bet.GolferId into dataGroup
               select new WinGroup()
               {
                   ID = dataGroup.Key,
                   Wins = dataGroup.Count()
               };
            return windata;
        }
        public IQueryable<PlaceGroup> CalculatePlaces()
        {
            IQueryable<PlaceGroup> placedata =
                from bet in _context.Bet
                where bet.Place == true
                group bet by bet.GolferId into dataGroup
                select new PlaceGroup()
                {
                    ID = dataGroup.Key,
                    Places = dataGroup.Count()
                };
            return placedata;
        }
        public IQueryable<ShowGroup> CalculateShows()
        {
            IQueryable<ShowGroup> showdata =
                from bet in _context.Bet
                where bet.Show == true
                group bet by bet.GolferId into dataGroup
                select new ShowGroup()
                {
                    ID = dataGroup.Key,
                    Shows = dataGroup.Count()
                };
            return showdata;
        }
        public IQueryable<GolferCard> CalculateHoles()
        {
            IQueryable<GolferCard> golfercardscore =
                from card in _context.Scoreboard
                orderby card.Hole ascending
                group card.Score by card.GolferName into dataGroup
                select new GolferCard()
                {
                    GolferName = dataGroup.Key,
                    ScoreList = dataGroup.ToList(),
                    HoleList = null
                };
            IQueryable<GolferCard> golfercardhole =
                from card in _context.Scoreboard
                orderby card.Hole ascending
                group card.Hole by card.GolferName into dataGroup
                select new GolferCard()
                {
                    GolferName = dataGroup.Key,
                    HoleList = dataGroup.ToList(),
                    ScoreList = null
                };
            IQueryable<GolferCard> golfercard = from gs in golfercardscore
                join gh in golfercardhole
                on gs.GolferName equals gh.GolferName
                select new GolferCard()
                {
                    GolferName = gs.GolferName,
                    HoleList = gh.HoleList,
                    ScoreList = gs.ScoreList
                };
            return golfercard;
        }
        public int SingleBetMoney( bool win, bool place, bool show)
        {
            int money=0;
            money = win == true ? money + 2 : money;
            money = place == true ? money + 2 : money;
            money = show == true ? money + 2 : money;
            return money;
        }
        public int CalculateBettorMoney()
        {
            int money = 0;
            foreach (var tb in _context.TempBet)
            {
                money = money + tb.Money;
            }
            return money;
        }
        public int CalculateMoneyDelete(int m, bool win, bool place, bool show)
        {
            int money = m;
            money = win == true ? money - 2 : money;
            money = place == true ? money - 2 : money;
            money = show == true ? money - 2 : money;
            return money;
        }
        /************SCORE BOARD******************/
        public async Task<IActionResult> ScoreBoard()
        {

            var scoreView = new ScoreBoardView();
            scoreView.GolferList = CalculateHoles().ToList();
            int total = 0;
            foreach (var s in scoreView.GolferList)
            {
                foreach(var sc in s.ScoreList)
                {
                    total = total + sc;
                }
                s.Total = total;
                total = 0;
            }
            return View(scoreView);
        }
        /************SCORE CARD******************/
        public async Task<IActionResult> ScoreCard()
        {
            var golfers = from g in _context.Golfer
                          where g.Playing == true
                          select g.GolferName;
            var scorecard_obj = new ScoreCard();
            scorecard_obj.Golfers= new SelectList(golfers);
            IEnumerable<int> h = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };
            scorecard_obj.Holes = new SelectList(h);
            IEnumerable<int> s = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            scorecard_obj.Scores = new SelectList(s);
            return View(scorecard_obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ScoreCard(string golferSelect, int holeSelect, int scoreSelect)
        {
            Scoreboard score_info = new Scoreboard { GolferName = golferSelect, Hole = holeSelect, Score = scoreSelect };
            var scoredata =
                from s in _context.Scoreboard
                where s.GolferName == golferSelect && s.Hole == holeSelect
                select s;
            if(scoredata.Count()>0)
            {
                scoredata.First().Score = scoreSelect;
                _context.Scoreboard.Update(scoredata.First());
            }
            else
            {
                _context.Scoreboard.Add(score_info);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ScoreCard));
        }

        /*************LOGIN******************/
        public async Task<IActionResult> LoginView()
        {

            return View();
        }
        
        public async Task<IActionResult> LoginVerify(string username, string password)
        {

            if (username == "admin" && password == "jamesp123")
            {
                HttpContext.Session.SetString("isAdmin", "true");
                ViewData["isAdmin"] = "true";
                return RedirectToAction(nameof(LaborDayMainView));
            }

            else
            {
                HttpContext.Session.SetString("isAdmin", "false");
                ViewData["isAdmin"] = "false";
                return RedirectToAction(nameof(LoginView));
            }
               
        }
        /*----------User Main View----------*/
        public async Task<IActionResult> UserMainView()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
            {
                HttpContext.Session.SetString("isAdmin", "false");
            }

            var golfers = from g in _context.Golfer
                          where g.Playing == true
                          select g;

            var bettors = from b in _context.Bettor
                          select b.Name;

            var LaborDayMV = new MainView();
            //Use this: SelectList Constructor (IEnumerable items, String dataValueField, String dataTextField)
            LaborDayMV.bettorList = new SelectList(bettors);
            LaborDayMV.golferList = await golfers.ToListAsync();
            LaborDayMV.WinList = await CalculateWins().ToListAsync();
            LaborDayMV.PlaceList = await CalculatePlaces().ToListAsync();
            LaborDayMV.ShowList = await CalculateShows().ToListAsync();
            if (_context.TempBet.Count() > 0)
            {
                foreach (var b in _context.TempBet)
                {
                    _context.TempBet.Remove(b);
                }
                await _context.SaveChangesAsync();
            }
            return View(LaborDayMV);
        }
        /* ------------DESKTOP MAIN VIEW ---------*/
        public async Task<IActionResult> DesktopMainView()
        {
            var golfers = from g in _context.Golfer
                          where g.Playing == true
                          select g;

            var bettors = from b in _context.Bettor
                          select b.Name;

            var LaborDayMV = new MainView();
            //Use this: SelectList Constructor (IEnumerable items, String dataValueField, String dataTextField)
            LaborDayMV.golferList = await golfers.ToListAsync();
            LaborDayMV.WinList = await CalculateWins().ToListAsync();
            LaborDayMV.PlaceList = await CalculatePlaces().ToListAsync();
            LaborDayMV.ShowList = await CalculateShows().ToListAsync();
            return View(LaborDayMV);

        }
        /*  -----------MAIN VIEW------------*/
        public async Task<IActionResult> LaborDayMainView()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
            {
                return View(nameof(LoginView)); 
            }
            var golfers = from g in _context.Golfer
                          where g.Playing == true
                          select g;

            var bettors = from b in _context.Bettor
                          select b.Name;
               
            var LaborDayMV = new MainView();
            //Use this: SelectList Constructor (IEnumerable items, String dataValueField, String dataTextField)
            LaborDayMV.bettorList = new SelectList(bettors);
            LaborDayMV.golferList = await golfers.ToListAsync();
            LaborDayMV.WinList = await CalculateWins().ToListAsync();
            LaborDayMV.PlaceList = await CalculatePlaces().ToListAsync();
            LaborDayMV.ShowList = await CalculateShows().ToListAsync();
            if (_context.TempBet.Count() > 0)
            {
                foreach(var b in _context.TempBet)
                {
                    _context.TempBet.Remove(b);
                }
                await _context.SaveChangesAsync();
            }
            return View(LaborDayMV);  
        }
        /* -----------Add Bettor------------*/
        public async Task<IActionResult> AddBettor(string bettorName)
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
            {
                return View(nameof(LoginView));
            }
            Bettor bettor = new Bettor  {Name=bettorName };
            _context.Bettor.Add(bettor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(LaborDayMainView));

        }
        /* -----------Add Golfer------------*/
        public async Task<IActionResult> AddGolfer(string golferName)
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
            {
                return View(nameof(LoginView));
            }
            Golfer golfer = new Golfer { GolferName = golferName, Playing = true };
            _context.Golfer.Add(golfer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AddGolferView));

        }
        public async Task<IActionResult> AddGolferView()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
            {
                return View(nameof(LoginView));
            }
            return View("AddGolfer");

        }
        /*  -----------Start Bet------------*/
        public async Task<IActionResult> StartBet(string bettorSelect,string golfername)
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
            {
                return View(nameof(LoginView));
            }
            var golfers = from g in _context.Golfer
                          where g.Playing == true
                          select g;

            var bettors = from b in _context.Bettor
                          select b;

            var sel_golfer = from g in _context.Golfer
                             where g.GolferName == golfername
                             select g;
            var sel_bettor = from b in _context.Bettor
                             where b.Name == bettorSelect
                             select b;

          
            ViewData["bettor_name"] = bettorSelect;
            ViewData["bettor_id"] = sel_bettor.First().ID;
            ViewData["golfer_name"] = golfername;
            ViewData["golfer_id"] = sel_golfer.First().ID;
            var LaborDayMV = new MainView();
            LaborDayMV.bettorList = new SelectList(await bettors.Distinct().ToListAsync());
            LaborDayMV.golferList = await golfers.ToListAsync();
            LaborDayMV.WinList = await CalculateWins().ToListAsync();
            LaborDayMV.PlaceList = await CalculatePlaces().ToListAsync();
            LaborDayMV.ShowList = await CalculateShows().ToListAsync();

            return View(LaborDayMV);
        }

        /*  -----------Place Initial Bet------------*/
        public async Task<IActionResult> PlaceSingleBet(int golferid, int bettorid, bool win, bool place, bool show)
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
            {
                return View(nameof(LoginView));
            }
            int money = SingleBetMoney(win, place, show);
            TempBet bet = new TempBet { GolferId = golferid, BettorID = bettorid, Win = win, Place = place, Show = show,Money=money};
            _context.TempBet.Add(bet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ContinueBets),new { bettor_id = bettorid });

        }

        // GET:
        public async Task<IActionResult> ContinueBets(int bettor_id)
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
            {
                return View(nameof(LoginView));
            }
            var golfers = from g in _context.Golfer
                          where g.Playing == true
                          select g;
 
            ViewData["bettor_id"] = bettor_id;
            var LaborDayMV = new MainView();
            LaborDayMV.golferList = await golfers.ToListAsync();
            LaborDayMV.TempBets = await _context.TempBet.ToListAsync();
            LaborDayMV.WinList = await CalculateWins().ToListAsync();
            LaborDayMV.PlaceList = await CalculatePlaces().ToListAsync();
            LaborDayMV.ShowList = await CalculateShows().ToListAsync();
            LaborDayMV.Money = CalculateBettorMoney();

            return View(LaborDayMV);

        }

        public async Task<IActionResult> ContinueBetsPost(string golfername,int bettorid)
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
            {
                return View(nameof(LoginView));
            }
            var golfers = from g in _context.Golfer
                          where g.Playing == true
                          select g;
            var sel_golfer = from g in _context.Golfer
                             where g.GolferName == golfername
                             select g;

            var LaborDayMV = new MainView();
            LaborDayMV.golferList = await golfers.ToListAsync();
            LaborDayMV.TempBets = await _context.TempBet.ToListAsync();
            LaborDayMV.WinList = await CalculateWins().ToListAsync();
            LaborDayMV.PlaceList = await CalculatePlaces().ToListAsync();
            LaborDayMV.ShowList = await CalculateShows().ToListAsync();
            LaborDayMV.Money = CalculateBettorMoney();

            ViewData["golfer_name"] = sel_golfer.First().GolferName;
            ViewData["golfer_id"] = sel_golfer.First().ID;
            ViewData["bettor_id"] = bettorid;
            return View(LaborDayMV);
        }

        public async Task<IActionResult> DeleteTemp(int id,int bettorid)
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
            {
                return View(nameof(LoginView));
            }
            var bet = await _context.TempBet.SingleOrDefaultAsync(m => m.ID == id);
            _context.TempBet.Remove(bet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ContinueBets),new { bettor_id = bettorid });
        }

        public async Task<IActionResult> CheckOut()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
            {
                return View(nameof(LoginView));
            }
            foreach ( var b in _context.TempBet)
            {
                Bet bet = new Bet { GolferId = b.GolferId, BettorID = b.BettorID, Win = b.Win, Place = b.Place, Show = b.Show };
                _context.Bet.Add(bet);
                _context.TempBet.Remove(b);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(LaborDayMainView));
        }

        // GET: Bets
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
            {
                return View(nameof(LoginView));
            }
            var laborDayContext = _context.Bet.Include(b => b.Bettor).Include(b => b.Golfer);
            return View(await laborDayContext.ToListAsync());
        }
        // GET: Bets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
            {
                return View(nameof(LoginView));
            }
            if (id == null)
            {
                return NotFound();
            }

            var bet = await _context.Bet
                .Include(b => b.Bettor)
                .Include(b => b.Golfer)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (bet == null)
            {
                return NotFound();
            }

            return View(bet);
        }
        public async Task<IActionResult> DeleteBettorsView(int? id)
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
            {
                return View(nameof(LoginView));
            }
            var bettors = from b in _context.Bettor
                          select b;

            List < Bettor > bettorList = await bettors.ToListAsync();
            return View(bettorList);
        }
        public async Task<IActionResult> DeleteBettor(int? id)
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
            {
                return View(nameof(LoginView));
            }
            var bettor = await _context.Bettor.SingleOrDefaultAsync(m => m.ID == id);
            _context.Bettor.Remove(bettor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DeleteBettorsView));
        }
        public async Task<IActionResult> DeleteGolfersView()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
            {
                return View(nameof(LoginView));
            }
            var golfers = from g in _context.Golfer
                          select g;
            List <Golfer> golferList = await golfers.ToListAsync();
            return View(golferList);
        }
        public async Task<IActionResult> DeleteGolfer(int ? id)
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
            {
                return View(nameof(LoginView));
            }
            var golfer = await _context.Golfer.SingleOrDefaultAsync(m => m.ID == id);
            _context.Golfer.Remove(golfer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DeleteGolfersView));
        }
        // POST: Bets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
            {
                return View(nameof(LoginView));
            }
            var bet = await _context.Bet.SingleOrDefaultAsync(m => m.ID == id);
            _context.Bet.Remove(bet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BetExists(int id)
        {
            return _context.Bet.Any(e => e.ID == id);
        }
         //GET
        public async Task<IActionResult> PayOut()
        {
            var golferdata = from golfer in _context.Golfer
                             select golfer.GolferName;
            Pay payView = new Pay();

            payView.Golfers = new SelectList(await golferdata.Distinct().ToListAsync());
            return View(payView);
        }
        public async Task<IActionResult> DeleteAll()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
            {
                return View(nameof(LoginView));
            }
            foreach (var b in _context.Bet)
            {
                _context.Bet.Remove(b);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeleteAllScoreCards()
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
            {
                return View(nameof(LoginView));
            }
            foreach (var g in _context.Scoreboard)
            {
                _context.Scoreboard.Remove(g);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> PayOutPost(string GolferWin, string GolferPlace, string GolferShow)
        {
            if (HttpContext.Session.GetString("isAdmin") != "true")
            {
                HttpContext.Session.SetString("isAdmin", "false");
            }
			ViewBag.Winner = GolferWin;
			ViewBag.Placer = GolferPlace;
			ViewBag.Shower = GolferShow;

            var winWinners = from bet in _context.Bet
                        where bet.Golfer.GolferName == GolferWin && bet.Win==true 
                        group bet by bet.BettorID into groupdata
                        select new WinGroup()
                        {
                            ID = groupdata.Key,
                            Wins = groupdata.Count()
                        };
			var winPlacers = from bet in _context.Bet
						where bet.Golfer.GolferName == GolferWin && bet.Place == true
						group bet by bet.BettorID into groupdata
						select new PlaceGroup()
						{
							ID = groupdata.Key,
							Places = groupdata.Count()
						};
			var winShowers = from bet in _context.Bet
						where bet.Golfer.GolferName == GolferWin && bet.Show == true
						group bet by bet.BettorID into groupdata
						select new ShowGroup()
						{
							ID = groupdata.Key,
							Shows = groupdata.Count()
						};
			var placePlacers = from bet in _context.Bet
                        where bet.Golfer.GolferName == GolferPlace && bet.Place == true
                        group bet by bet.BettorID into groupdata
                        select new PlaceGroup()
                        {
                            ID = groupdata.Key,
                            Places = groupdata.Count()
                        };
			var placeShowers = from bet in _context.Bet
						where bet.Golfer.GolferName == GolferPlace && bet.Show == true
						group bet by bet.BettorID into groupdata
						select new ShowGroup()
						{
							ID = groupdata.Key,
							Shows = groupdata.Count()
						};
			var showShowers = from bet in _context.Bet
                        where bet.Golfer.GolferName == GolferShow && bet.Show == true
                        group bet by bet.BettorID into groupdata
                        select new ShowGroup()
                        {
                            ID = groupdata.Key,
                            Shows = groupdata.Count()
                        };
            var bettors = from bettor in _context.Bettor
                          select bettor;

            var LaborDayMV = new MainView();
            LaborDayMV.WinWinnersList = await winWinners.ToListAsync();
			LaborDayMV.WinPlacersList = await winPlacers.ToListAsync();
			LaborDayMV.WinShowersList = await winShowers.ToListAsync();
			LaborDayMV.PlacePlacersList = await placePlacers.ToListAsync();
			LaborDayMV.PlaceShowersList = await placeShowers.ToListAsync();
			LaborDayMV.ShowShowersList = await showShowers.ToListAsync();
            LaborDayMV.ListOfBettors = await bettors.ToListAsync();
            return View(LaborDayMV);
        }
    }

}

        

        
/*
        // GET: Bets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bet = await _context.Bet
                .Include(b => b.Bettor)
                .Include(b => b.Golfer)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (bet == null)
            {
                return NotFound();
            }

            return View(bet);
        }

        // GET: Bets/Create
        public IActionResult Create()
        {
            ViewData["BettorID"] = new SelectList(_context.Bettor, "ID", "ID");
            ViewData["GolferId"] = new SelectList(_context.Golfer, "ID", "ID");
            return View();
        }

        // POST: Bets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,GolferId,BettorID,Win,Place,Show")] Bet bet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BettorID"] = new SelectList(_context.Bettor, "ID", "ID", bet.BettorID);
            ViewData["GolferId"] = new SelectList(_context.Golfer, "ID", "ID", bet.GolferId);
            return View(bet);
        }

        // GET: Bets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bet = await _context.Bet.SingleOrDefaultAsync(m => m.ID == id);
            if (bet == null)
            {
                return NotFound();
            }
            ViewData["BettorID"] = new SelectList(_context.Bettor, "ID", "ID", bet.BettorID);
            ViewData["GolferId"] = new SelectList(_context.Golfer, "ID", "ID", bet.GolferId);
            return View(bet);
        }

        // POST: Bets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,GolferId,BettorID,Win,Place,Show")] Bet bet)
        {
            if (id != bet.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BetExists(bet.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BettorID"] = new SelectList(_context.Bettor, "ID", "ID", bet.BettorID);
            ViewData["GolferId"] = new SelectList(_context.Golfer, "ID", "ID", bet.GolferId);
            return View(bet);
        }

        
    */
        