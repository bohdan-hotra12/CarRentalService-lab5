using CarRentalServiceLab3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CarRentalServiceLab3
{
    public static class FileHelper
  


    {
        public static bool FileHasCorrectHeader(string path, string expectedHeader)
        {
            if (!File.Exists(path)) return false;
            var lines = File.ReadAllLines(path);
            if (lines.Length == 0) return false;
            return lines[0].Trim() == expectedHeader.Trim();
        }

        public static List<string[]> ReadCsvLines(string path, string expectedHeader)
        {
            var result = new List<string[]>();

            if (!File.Exists(path))
            {
                // Створюємо файл з шапкою, якщо його немає
                File.WriteAllText(path, expectedHeader + Environment.NewLine);
                return result;
            }

            var lines = File.ReadAllLines(path);
            if (lines.Length == 0 || lines[0] != expectedHeader)
            {
                Console.WriteLine($"Помилка: файл {path} має неправильну структуру або відсутня шапка!");
                return result;
            }

            for (int i = 1; i < lines.Length; i++) // пропускаємо шапку
            {
                var line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(',');
                result.Add(parts);
            }

            return result;
        }

        public static void AppendToCsv(string path, string line)
        {
            File.AppendAllText(path, line + Environment.NewLine);
        }

        public static void RewriteCsv(string path, IEnumerable<string> lines)
        {
            File.WriteAllLines(path, lines);
        }
    }

    public static class IdGenerator
    {
        public static int GetNextId(string path, string header)
        {
            if (!File.Exists(path))
                return 1;

            var lines = File.ReadAllLines(path).Skip(1); // пропускаємо шапку

            int maxId = 0;
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(',');
                if (parts.Length > 0 && int.TryParse(parts[0], out int id))
                {
                    if (id > maxId) maxId = id;
                }
            }

            return maxId + 1;
        }
    }

    // Простий хеш пароля (MD5 — для навчальних цілей, в реальності використовувати BCrypt або Argon2)
    public static class PasswordHasher
    {
        public static string Hash(string password)
        {
            using (var md5 = MD5.Create())
            {
                var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        public static bool Verify(string password, string hash)
        {
            return Hash(password) == hash;
        }
    }
}
