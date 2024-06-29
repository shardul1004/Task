using MatchBall.Services;

namespace MatchBall.Utilities
{
    public interface IJWTUtil
    {
        public Status GenerateToken(string token, int expiry);
        public bool ValidateToken(string token);
        public Status FetchUsername();
        public bool ValidateRefreshToken(string JWTToken, string RefreshToken);

    }
}
