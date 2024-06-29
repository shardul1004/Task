using MatchBall.ApplicationContext;
using MatchBall.Filter;
using MatchBall.Services;
using MatchBall.Utilities;
using MatchBall.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Identity.Client;

namespace MatchBall.Controllers
{
    [ServiceFilter(typeof(JWTAuth))]
    public class HomePageController : Controller
    {
        private readonly IBallMachine BallMachine;
        private readonly IJWTUtil JWTUtil;
        private readonly IMemoryCache Cache;
        private readonly TestDbContext DbContext;
        private readonly IAccounts Account;
        public HomePageController(IBallMachine BallMachine, IJWTUtil JWTUtil, IMemoryCache Cache, TestDbContext DbContext, IAccounts Account)
        {
            this.BallMachine = BallMachine;
            this.JWTUtil = JWTUtil;
            this.Cache = Cache;
            this.DbContext = DbContext;
            this.Account = Account;
        }
        public IActionResult HomePageView(HomePageViewModel HomePageViewModel)
        {
            Status FetchedUsername = JWTUtil.FetchUsername();
            if (!FetchedUsername.Flag)
            {
                return RedirectToAction("LoginPage", "Login");
            }
            string Username = FetchedUsername.message;
            int userId;
            string key = $"UserIdfrom_{Username}";
            if (Cache.TryGetValue(key, out int CacheduserId))
            {
                userId = CacheduserId;
            }
            else
            {
                Usertableballgame UserTableBallGame =DbContext.Usertableballgames.Where(item => item.Username == Username).FirstOrDefault();
                userId = UserTableBallGame.Userid;
                var CacheEntryOption = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                Cache.Set(key, userId, CacheEntryOption);
            }
            HomePageViewModel.userId = userId;
            return View("HomePageView", HomePageViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> ThrowBall(int id)
        {
            Random Random = new Random();
            Status FetchedUsername = JWTUtil.FetchUsername();
            if (!FetchedUsername.Flag) {
                return RedirectToAction("LoginPage", "Login");
            }
            string Username = FetchedUsername.message;
            int userId;
            string key = $"UserIdfrom_{Username}";
            if (Cache.TryGetValue(key, out int CacheduserId)) {
                userId = CacheduserId;
            }
            else
            {
                Usertableballgame UserTableBallGame = await DbContext.Usertableballgames.Where(item => item.Username == Username).FirstOrDefaultAsync();
                userId = UserTableBallGame.Userid;
                var CacheEntryOption = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                Cache.Set(key, userId, CacheEntryOption);
            }
            
            if (ColorKeeper.color == 0){
                ColorKeeper.color = Random.Next(1, 5);
            }

            Status result = await BallMachine.Matching(id, userId);
            if (result.Flag)
            {
                return ResultPage( new ResultPageViewModel { result=result.message});
            }
            else
            {
                return HomePageView(new HomePageViewModel { ErrorText = result.message});
            }

        }

        public async Task<IActionResult> AddCredit(int amount)
        {
            if (amount % 5 != 0)
            {
                return HomePageView(new HomePageViewModel { ErrorText = "Please Enter Value which is multiple of 5" });
            }
            amount = amount / 5;
            Status UsernameRes = JWTUtil.FetchUsername();
            
            if (!UsernameRes.Flag)
            {
                RedirectToAction("LoginPage", "Login");
            }
            string userName = UsernameRes.message;
            int userId;
            string key = $"UserIdfrom_{userName}";
            if (Cache.TryGetValue(key, out int CacheduserId))
            {
                userId = CacheduserId;
            }
            else
            {
                Usertableballgame UserTableBallGame = await DbContext.Usertableballgames.Where(item => item.Username == userName).FirstOrDefaultAsync();
                userId = UserTableBallGame.Userid;
                var CacheEntryOption = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                Cache.Set(key, userId, CacheEntryOption);
            }

            Accountballgame accountBallGame = await DbContext.Accountballgames.FirstOrDefaultAsync(a => a.Userid == userId);
            if (accountBallGame == null)
            {
                return HomePageView(new HomePageViewModel { ErrorText = "Account not found" });
            }

            Status result = Account.AddCredits(amount, accountBallGame);

            if (result.Flag)
            {
                return HomePageView(new HomePageViewModel());
            }
            else
            {
                return HomePageView(new HomePageViewModel { ErrorText = result.message });
            }
        }

        public async Task<IActionResult> WithdrawCredit(int amount)
        {
            Status UsernameRes = JWTUtil.FetchUsername();

            if (!UsernameRes.Flag)
            {
                RedirectToAction("LoginPage", "Login");
            }
            string userName = UsernameRes.message;
            int userId;
            string key = $"UserIdfrom_{userName}";
            if (Cache.TryGetValue(key, out int CacheduserId))
            {
                userId = CacheduserId;
            }
            else
            {
                Usertableballgame UserTableBallGame = await DbContext.Usertableballgames.Where(item => item.Username == userName).FirstOrDefaultAsync();
                userId = UserTableBallGame.Userid;
                var CacheEntryOption = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                Cache.Set(key, userId, CacheEntryOption);
            }

            var accountBallGame = await DbContext.Accountballgames.FirstOrDefaultAsync(a => a.Userid == userId);
            if (accountBallGame == null)
            {
                return HomePageView(new HomePageViewModel { ErrorText = "Account not found" });
            }

            Status result = Account.DecrementCredits(amount, accountBallGame);

            if (result.Flag)
            {
                return HomePageView(new HomePageViewModel());
            }
            else
            {
                return HomePageView(new HomePageViewModel { ErrorText = result.message });
            }
        }

        public IActionResult ResultPage(ResultPageViewModel result)
        {
            return View("ResultPage",result);
        }
    }
}
