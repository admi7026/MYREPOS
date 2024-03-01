using System.Security.Cryptography;
using System.Text;

namespace Account.API.Models
{
    public class Md5Helper
    {
        public static string GetMd5Hash(string text)
        {
            using var md5 = MD5.Create();

            md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            
            byte[] result = md5.Hash!;
            
            StringBuilder str = new StringBuilder();
            
            for (int i = 0; i < result.Length; i++)
            {
                str.Append(result[i].ToString("x2"));
            }
            
            return str.ToString();
        }

        public static bool VerifyMd5Hash(string plainText, string hashVerify)
        {
            var inputHash = GetMd5Hash(plainText);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(inputHash, hashVerify))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
