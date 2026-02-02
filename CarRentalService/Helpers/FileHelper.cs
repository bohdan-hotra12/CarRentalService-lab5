using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalService.Helpers
{
    public static class FileHelper
    {
        public static List<string[]> ReadCsvLines(string path, string expectedHeader)
        {
            var result = new List<string[]>();

            if (!File.Exists(path))
            {
                File.WriteAllText(path, expectedHeader + Environment.NewLine);
                return result;
            }

            var lines = File.ReadAllLines(path);
            if (lines.Length == 0 || lines[0] != expectedHeader)
            {
                Console.WriteLine($"Помилка: файл {path} має неправильну структуру!");
                return result;
            }

            for (int i = 1; i < lines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                    result.Add(lines[i].Split(','));
            }

            return result;
        }

        public static void RewriteCsv(string path, IEnumerable<string> lines)
        {
            File.WriteAllLines(path, lines);
        }
    }
}
