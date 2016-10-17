using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace nant.Authentication
{
    public class PasswordHasher
    {
        // from: http://stackoverflow.com/questions/2138429/hash-and-salt-passwords-in-c-sharp

        public static string GenerateSaltedHash(string plainText, string salt)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var saltBytes = Encoding.UTF8.GetBytes(salt);

            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainTextBytes[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = saltBytes[i];
            }

            return Encoding.UTF8.GetString(algorithm.ComputeHash(plainTextWithSaltBytes));
        }
    }
}
