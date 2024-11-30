﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Talabat.Domain.Layer.Entities.Identity
{
    public class Address
    {
        public int Id { get; set; }
        
        public string Street { get; set; }
        
        public string City { get; set; }
        
        public string Country { get; set; }
        
        public string State { get; set; }
        
        public string ZipCode { get; set; }

        // name of person who lives in this address and can receive orders

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }

        public ApplicationUser AppUser { get; set; }
    }
}