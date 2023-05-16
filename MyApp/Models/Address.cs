using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyApp.Models
{
    public class Address
    {
        public string Street { get; set; }
        public int Number { get; set; }
        public string City { get; set; }
        public int PostCode { get; set; }

        public Address()
        {

        }

        public override string ToString()
        {
            return $"{Street} {Number}, {City}, {PostCode}";
        }
    }
}