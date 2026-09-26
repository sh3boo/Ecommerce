using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Commands
{
    public class CheckOutOrderCommandV2 : IRequest<int>
    {
        public string? UserName { get; set; }
        public decimal? TotaPrice { get; set; }
    }
}
