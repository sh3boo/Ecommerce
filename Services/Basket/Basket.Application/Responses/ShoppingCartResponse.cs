using Basket.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Responses
{
    public class ShoppingCartResponse
    {
        public string UserName { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
        public ShoppingCartResponse()
        {

        }
        public ShoppingCartResponse(string userName)
        {
            UserName = userName;
        }
        public decimal TotalPrice 
        { get
            {
                decimal TotalPrice = 0;
                foreach (ShoppingCartItem item in Items)
                {
                    TotalPrice += item.Price * item.Quantity;
                }
                return TotalPrice;
            } 
        }
    }
}
