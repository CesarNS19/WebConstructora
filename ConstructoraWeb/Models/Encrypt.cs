using System.Security.Cryptography;
using System.Text;

public class Encrypt
{
    public static string GetSHA256(string str, string salt)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            string saltedStr = str + salt;
            byte[] bytes = Encoding.UTF8.GetBytes(saltedStr);
            byte[] hash = sha256.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }
    }
}
