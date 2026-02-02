using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalService.Models
{
    struct VehicleSummary
    {
        public string Model;
        public string Type;
        public int PricePerDay;
        public bool IsAvailable;

        public VehicleSummary(string model, string type, int pricePerDay, bool isAvailable)
        {
            Model = model;
            Type = type;
            PricePerDay = pricePerDay;
            IsAvailable = isAvailable;
        }
    }
}
