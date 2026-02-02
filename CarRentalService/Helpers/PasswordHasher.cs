using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalService.Helpers
{
    public static class PasswordHasher
    {
        public static string Hash(string password)
        {
            using var md5 = MD5.Create();
            return BitConverter.ToString(md5.ComputeHash(
                Encoding.UTF8.GetBytes(password)))
                .Replace("-", "").ToLower();
        }
    }
}
