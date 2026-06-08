using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Core.Entites
{
    public class BasketCheckout
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string CartName { get; set; }
        public string CartNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public int paymentMethod { get; set; }


    }
}
