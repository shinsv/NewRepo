using ECommerce.Models;

namespace ECommerce.Contracts
{
    public interface IOrderRepository
    {
        Task<Order> GetMostRecentOrderAsync(Request request);

    }
}
