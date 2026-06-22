using Basket.Core.Entites;
using Basket.Core.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task<ShoppingCart?> GetBasket(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return null;

            var basketJson =
                await _redisCache.GetStringAsync(userName);

            if (string.IsNullOrWhiteSpace(basketJson))
                return null;

            return JsonConvert.DeserializeObject<ShoppingCart>(
                basketJson);
        }

        public async Task<ShoppingCart> UpdateBasket(
            ShoppingCart cart)
        {
            if (cart == null)
                throw new ArgumentNullException(nameof(cart));

            if (string.IsNullOrWhiteSpace(cart.UserName))
            {
                throw new ArgumentException(
                    "UserName is required.",
                    nameof(cart));
            }

            var basketJson =
                JsonConvert.SerializeObject(cart);

            await _redisCache.SetStringAsync(
                cart.UserName,
                basketJson);

            var savedBasket =
                await GetBasket(cart.UserName);

            if (savedBasket == null)
            {
                throw new InvalidOperationException(
                    $"Basket '{cart.UserName}' was not saved in Redis.");
            }

            return savedBasket;
        }

        public async Task DeleteBasket(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return;

            await _redisCache.RemoveAsync(userName);
        }
    }
}