using System.Security.Cryptography;
using System.Text;

namespace Airline.Data.Helpers
{
    public static class HashHelper
    {
        public static string ComputeSha256Hash(string raw)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(raw));
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
