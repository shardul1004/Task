using System.Security.Cryptography;

namespace MatchBall.Utilities
{
    public class Hashing: IHashing
    {
        public Hashing() { }

        public string Hash(string key, byte[] salt)
        {
            var pbfk2 = new Rfc2898DeriveBytes(key,salt,1000);
            byte[] hash = pbfk2.GetBytes(36);
            string hashedKey = Convert.ToBase64String(hash);
            return hashedKey;
        }
    }
}
