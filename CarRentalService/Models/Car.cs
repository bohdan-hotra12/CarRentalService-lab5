using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalService.Models
{
    class Car
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public int PricePerDay { get; set; }
        public bool IsAvailable { get; set; }
        public int RentedByClientId { get; set; }
        public DateTime? RentedFrom { get; set; }
        public int RentedDays { get; set; }

        public Car(int id, string model, string type, int pricePerDay, bool isAvailable = true)
        {
            Id = id;
            Model = model;
            Type = type;
            PricePerDay = pricePerDay;
            IsAvailable = isAvailable;
        }

        public override string ToString()
        {
            string status = IsAvailable
                ? "Доступне"
                : $"Орендоване (ID клієнта: {RentedByClientId})";

            return $"{Model} ({Type}) - {PricePerDay} грн/день - {status}";
        }
    }
}
