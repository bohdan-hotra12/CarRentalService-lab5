using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalService.Models
{
    class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string ID { get; set; }
        public int Age { get; set; }

        public Client(string name, string phone, string id, int age)
        {
            Name = name;
            Phone = phone;
            ID = id;
            Age = age;
        }
    }
}
