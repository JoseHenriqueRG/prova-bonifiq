using ProvaPub.Models;

namespace ProvaPub.Services.Orders
{
    public interface IOrderService
    {
        Task<Order> PayOrder(string paymentMethodName, decimal paymentValue, int customerId);
    }
}
