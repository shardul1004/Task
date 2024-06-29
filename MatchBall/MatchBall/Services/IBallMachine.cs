namespace MatchBall.Services
{
    public interface IBallMachine
    {
        public Task<Status> Matching(int InputColor, int UserId);
    }
}
