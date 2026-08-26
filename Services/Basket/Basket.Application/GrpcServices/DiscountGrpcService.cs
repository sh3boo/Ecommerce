using Discount.Grpc.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountGrpcClient;
        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountGrpcClient)
        {
            _discountGrpcClient = discountGrpcClient;            
        }
        public async Task<CouponModel> GetDiscount(string productName)
        {
            var dicountRequest=new GetDiscountRequest { ProductName = productName };
            return await _discountGrpcClient.GetDiscountAsync(dicountRequest);
        }
    }
}
