using CarRentalService.Helpers;
using CarRentalService.Models;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalService.Services
{
    class CarServices
    {
        private const string CarsFile = "cars.csv";
        private const string CarsHeader = "Id,Model,Type,PricePerDay,IsAvailable,RentedByClientId,RentedFrom,RentedDays";
        public static List<Car> LoadCars()
        {
            var carList = new List<Car>();
            var lines = FileHelper.ReadCsvLines(CarsFile, CarsHeader);

            foreach (var parts in lines)
            {
                if (parts.Length < 8) continue;

                if (int.TryParse(parts[0], out int id) &&
                    int.TryParse(parts[3], out int price) &&
                    bool.TryParse(parts[4], out bool available) &&
                    int.TryParse(parts[5], out int rentedById) &&
                    int.TryParse(parts[7], out int days))
                {
                    var car = new Car(id, parts[1], parts[2], price, available)
                    {
                        RentedByClientId = rentedById,
                        RentedDays = days
                    };

                    if (!string.IsNullOrEmpty(parts[6]) && DateTime.TryParse(parts[6], out DateTime rentedFrom))
                        car.RentedFrom = rentedFrom;

                    carList.Add(car);
                }
            }

            return carList;
        }

        public static void SaveCars(List<Car> carsToSave)
        {
            var lines = new List<string> { CarsHeader };

            foreach (var car in carsToSave)
            {
                string rentedFromStr = car.RentedFrom.HasValue ? car.RentedFrom.Value.ToString("yyyy-MM-dd") : "";
                string line = $"{car.Id},{car.Model},{car.Type},{car.PricePerDay},{car.IsAvailable},{car.RentedByClientId},{rentedFromStr},{car.RentedDays}";
                lines.Add(line);
            }

            FileHelper.RewriteCsv(CarsFile, lines);
        }
    }
}
