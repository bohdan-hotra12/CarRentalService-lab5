using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalService.Models
{
    class Booking
    {
        public Car Car { get; set; }
        public Client Client { get; set; }
        public DateTime From { get; set; }
        public int Days { get; set; }

        public decimal TotalPrice => Car.PricePerDay * Days;

        public Booking(Car car, Client client, DateTime from, int days)
        {
            Car = car;
            Client = client;
            From = from;
            Days = days;
        }

        public override string ToString()
        {
            return $"{Car.Model} → {Client.Name}, з {From:dd.MM.yyyy} на {Days} дн. ({TotalPrice} грн)";
        }
    }
}
