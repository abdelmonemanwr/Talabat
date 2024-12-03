using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Domain.Layer.Entities.Order_Aggregate
{
    public class Address
    {
        public Address() { }
      
        public Address(int building, string street, string city, string country, string state, string zipCode, string firstName)
        {
            Building = building;
            Street = street;
            City = city;
            Country = country;
            State = state;
            ZipCode = zipCode;
            FirstName = firstName;
        }
        
        public string City { get; set; }

        public string State { get; set; }

        public int Building { get; set; }

        public string Street { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }

        public string FirstName { get; set; }
    }
}
