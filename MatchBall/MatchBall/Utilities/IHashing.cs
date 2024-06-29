using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MatchBall.Utilities
{
    public interface IHashing
    {
        public string Hash(string key, byte[] salt);
    }
}
