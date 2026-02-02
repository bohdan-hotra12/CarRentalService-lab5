using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalService.Helpers
{
    public static class IdGenerator
    {
        public static int GetNextId(string path, string header)
        {
            if (!File.Exists(path)) return 1;

            int maxId = 0;
            foreach (var line in File.ReadAllLines(path).Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = line.Split(',');
                if (int.TryParse(parts[0], out int id) && id > maxId)
                    maxId = id;
            }
            return maxId + 1;
        }
    }
}
