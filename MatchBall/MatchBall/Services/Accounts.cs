using Microsoft.EntityFrameworkCore;
using MatchBall.ApplicationContext;
using Microsoft.IdentityModel.Tokens;
namespace MatchBall.Services
{
    public class Accounts: IAccounts
    {
        private readonly TestDbContext DbContext;
        public Accounts(TestDbContext DbContext)
        {
            this.DbContext = DbContext;
        }

        public Status AddCredits(int Credits, Accountballgame AccountBallGame) {
            int NewCredit = AccountBallGame.Credit;
            try
            {
                checked
                {
                    NewCredit += Credits;
                }
                AccountBallGame.Credit = NewCredit;
                DbContext.Accountballgames.Update(AccountBallGame);
                DbContext.SaveChanges();
                return new Status { Flag = true, message = "Success" };
            }
            catch (OverflowException)
            {
                return new Status { Flag = false, message = "Please withdraw credits" };
            }
            catch (Exception ex) {
                return new Status { Flag = false, message = "Please try again" };
            }
        }

        public Status DecrementCredits(int Credits, Accountballgame AccountBallGame) {
            int NewCredits = AccountBallGame.Credit;
            try
            {
                NewCredits -= Credits;
                if (NewCredits < 0 || NewCredits>AccountBallGame.Credit)
                {
                    return new Status { Flag = false, message = "Insufficient Credits" };
                }
                else
                {
                    AccountBallGame.Credit = NewCredits;
                    DbContext.Accountballgames.Update(AccountBallGame);
                    DbContext.SaveChanges();

                    return new Status { Flag = true, message = "Success" };
                }
            }
            catch (Exception ex) {
                return new Status { Flag = false, message = "Please try again" };
            }
        }

        public int GetCreditByUserId(int userId)
        {
            try
            {
                Accountballgame AccountBallGame = DbContext.Accountballgames.Where(item => item.Userid == userId).FirstOrDefault();
                if (AccountBallGame == null)
                {
                    return 0;
                }
                else
                {
                    return AccountBallGame.Credit ;
                }
            }
            catch (Exception ex) {
                return 0;
            }
        }
    }
}
