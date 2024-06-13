using System.Security.Cryptography;
using System.Text;

namespace backend2.Criptografia {
    public class GeneratePassword {

        public static string HashPassword(string password) {
            using (SHA1 sha1Hash = SHA1.Create()) {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha1Hash.ComputeHash(sourceBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                return hash;
            }
        }
    }
}
