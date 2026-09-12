using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Core.Entites
{
    public class Order:EntityBase
    {
        public string? UserName { get; set; }
        public decimal? TotaPrice { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string ? EmailAddress { get; set; }
        public string? AddressLine { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Phone { get; set; }
        public string? CardName { get; set; }
        public string? CardNumber { get; set; }
        public string? Expiration { get; set; }
        public string? cvv { get; set; }
        public int? PaymentMethod { get; set; }


    }
}
