using MatchBall.ApplicationContext;
using Microsoft.EntityFrameworkCore;

namespace MatchBall.Services
{
    public class BallMachine: IBallMachine
    {
        private readonly int color;
        private readonly IAccounts Accounts;
        private readonly TestDbContext dbContext;
        private readonly Random Random;
        public BallMachine(IAccounts Accounts, TestDbContext dbContext)
        {
            this.color = ColorKeeper.color;
            this.Accounts = Accounts;
            this.dbContext = dbContext;
            Random = new Random();
        }

        public async Task<Status> Matching(int InputColor, int UserId)
        {
            var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                Accountballgame accountballgame = await dbContext.Accountballgames.Where(item => item.Userid == UserId).FirstOrDefaultAsync();
                if (accountballgame == null) {
                    transaction.Commit();
                    return new Status { Flag = false, message="No User found" };
                }
                
                int Credits = Accounts.GetCreditByUserId(UserId);
                if (Credits == 0)
                {
                        transaction.Commit();
                        return new Status { Flag = false, message = "Insufficient Balance" };
                    }
                    else
                    {
                        if (InputColor == color) {
                            Accounts.AddCredits(1, accountballgame);
                            transaction.Commit();
                            return new Status { Flag = true, message = "Won" };
                        }
                        else
                        {
                            Accounts.DecrementCredits(1, accountballgame); 
                            transaction.Commit();
                            return new Status { Flag = true, message = "Lost" };
                        }
                        
                    }
                
            }
            catch (Exception ex) {
                transaction.Rollback();
                return new Status { Flag = false, message= "Please try again" };
            }
            finally
            {
                ColorKeeper.color = Random.Next(1, 5);
            }
        }
    }
}
