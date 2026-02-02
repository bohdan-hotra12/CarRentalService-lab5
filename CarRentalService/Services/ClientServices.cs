using CarRentalService.Helpers;
using CarRentalService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalService.Services
{
    class ClientServices
    {
        public static List<Client> LoadClients()
        {
            var clientList = new List<Client>();
            var lines = FileHelper.ReadCsvLines("clients.csv", "Id,Name,Phone,ID,Age");

            foreach (var parts in lines)
            {
                if (parts.Length < 5) continue;

                if (int.TryParse(parts[0], out int id) &&
                    int.TryParse(parts[4], out int age))
                {
                    clientList.Add(new Client(parts[1], parts[2], parts[3], age) { Id = id });
                }
            }

            return clientList;
        }
        public static void SaveClients(List<Client> clientsToSave)
        {
            string clientsFile = "clients.csv";
            string header = "Id,Name,Phone,ID,Age";

            var lines = new List<string> { header };

            foreach (var cl in clientsToSave)
            {
                string line = $"{cl.Id},{cl.Name.Replace(",", "")},{cl.Phone.Replace(",", "")},{cl.ID.Replace(",", "")},{cl.Age}";
                lines.Add(line);
            }

            FileHelper.RewriteCsv(clientsFile, lines);
        }
    }
}
