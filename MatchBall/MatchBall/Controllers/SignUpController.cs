using MatchBall.ApplicationContext;
using MatchBall.Utilities;
using MatchBall.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace MatchBall.Controllers
{
    public class SignUpController : Controller
    {
        private readonly IHashing Hashing;
        private readonly TestDbContext TestDbContext;
        public SignUpController(IHashing Hashing, TestDbContext TestDbContext)
        {
            this.Hashing = Hashing;
            this.TestDbContext = TestDbContext;
        }
        public IActionResult SignUpPage(LoginViewModel SignUpModel)
        {
            return View("SignUpPage", SignUpModel);
        }

        public async Task<IActionResult> Register(LoginViewModel SignUpModel)
        {
            if (ModelState["Username"].Errors.FirstOrDefault() != null || ModelState["Password"].Errors.FirstOrDefault() != null)
            {
                SignUpModel.ErrorText = "Please enter required field";
                return SignUpPage(SignUpModel);
            }
            else {
                if (TestDbContext.Usertableballgames.Where(item => item.Username == SignUpModel.Username).FirstOrDefault() != null) {
                    SignUpModel.ErrorText = "Please select another username";
                    return SignUpPage(SignUpModel);
                }
                RandomNumberGenerator rng = RandomNumberGenerator.Create();
                byte[] salt = new byte[16];
                rng.GetBytes(salt);
                string hashedPassword = Hashing.Hash(SignUpModel.Password, salt);
                Usertableballgame usertableballgame = new Usertableballgame
                {
                    Username = SignUpModel.Username,
                    Hpassword = hashedPassword,
                    Salt = Convert.ToBase64String(salt)
                };
                TestDbContext.Usertableballgames.Add(usertableballgame);
                await TestDbContext.SaveChangesAsync();
                TestDbContext.Accountballgames.Add(new Accountballgame {
                    Userid = usertableballgame.Userid,
                    Credit = 5
                });
                await TestDbContext.SaveChangesAsync();
                return RedirectToAction("LoginPage", "Login");
            }

        }
    }
}
