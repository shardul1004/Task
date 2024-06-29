using MatchBall.ApplicationContext;
using MatchBall.Utilities;
using MatchBall.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MatchBall.Services;

namespace MatchBall.Controllers
{
    public class LoginController : Controller
    {
        private readonly TestDbContext DbContext;
        private readonly IHashing Hashing;
        private readonly IJWTUtil JWTUtil;
        private readonly int ExpiryJWT;
        private readonly int ExpiryRefresh;
        public LoginController(TestDbContext DbContext, IHashing Hashing, IJWTUtil JWTUtil, IConfiguration Configuration) {
            this.DbContext = DbContext;
            this.Hashing = Hashing;
            this.JWTUtil = JWTUtil;
            ExpiryJWT = int.Parse(Configuration["JWT:ExpiryJWT"]);
            ExpiryRefresh = int.Parse(Configuration["JWT:ExpiryRefresh"]);
        }
        public IActionResult LoginPage(LoginViewModel loginViewModel)
        {
            List<Usertableballgame> li1 = DbContext.Usertableballgames.Where(item => true).ToList();
            List<Accountballgame> li = DbContext.Accountballgames.Where(item => true).ToList();
            return View("LoginPage",loginViewModel);
        }

        public async Task<IActionResult> Authenticate(LoginViewModel LoginData)
        {
            if (ModelState["Username"].Errors.FirstOrDefault() != null || ModelState["Password"].Errors.FirstOrDefault() != null)
            {
                LoginData.ErrorText = "Please enter required field";
                return LoginPage(LoginData);
            }
            else
            {
                Usertableballgame UserTableBallGame = await DbContext.Usertableballgames.Where(item => item.Username == LoginData.Username).FirstOrDefaultAsync();
                if (UserTableBallGame == null) {
                    LoginData.ErrorText = "Please enter valid credentials";
                    return LoginPage(LoginData);
                }
                else
                {
                    byte[] salt = Convert.FromBase64String(UserTableBallGame.Salt);
                    string HashedPassword = Hashing.Hash(LoginData.Password, salt);
                    if (HashedPassword == UserTableBallGame.Hpassword) {
                        Status result = JWTUtil.GenerateToken(LoginData.Username, ExpiryJWT);
                        if (result.Flag)
                        {
                            Response.Cookies.Append("Token", result.message);
                        }
                        else
                            {
                            LoginData.ErrorText = "Please Try again";
                            return LoginPage(LoginData);
                        }
                        result = JWTUtil.GenerateToken(LoginData.Username, ExpiryRefresh);
                        if (result.Flag)
                        {
                            Response.Cookies.Append("RefreshToken", result.message);
                        }
                        else
                        {
                            LoginData.ErrorText = "Please Try again";
                            return LoginPage( LoginData);
                        }
                        return RedirectToAction("HomePageView", "HomePage");
                    }
                    else
                    {
                        LoginData.ErrorText = "Please enter valid credentials";
                        return LoginPage( LoginData);
                    }
                }
            }
        }
    }
}
