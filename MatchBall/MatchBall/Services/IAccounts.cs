using MatchBall.ApplicationContext;

namespace MatchBall.Services
{
    public interface IAccounts
    {
        public Status AddCredits(int credits, Accountballgame AccountBallGame);
        public Status DecrementCredits(int credits, Accountballgame AccountBallGame);
        public int GetCreditByUserId(int userId);
    }
}
