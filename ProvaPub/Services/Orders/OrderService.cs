using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly TestDbContext _ctx;
        private readonly IEnumerable<PaymentMethods.IPaymentMethod> _paymentMethods;

        public OrderService(TestDbContext ctx, IEnumerable<PaymentMethods.IPaymentMethod> paymentMethods)
        {
            _ctx = ctx;
            _paymentMethods = paymentMethods;
        }

        public async Task<Order> PayOrder(string paymentMethodName, decimal paymentValue, int customerId)
        {
            var paymentMethod = _paymentMethods.FirstOrDefault(pm => pm.Name.Equals(paymentMethodName, StringComparison.OrdinalIgnoreCase));

            if (paymentMethod == null)
            {
                throw new ArgumentException($"Payment method '{paymentMethodName}' not supported.");
            }

            paymentMethod.ProcessPayment(paymentValue, customerId);

            return await InsertOrder(new Order()
            {
                Value = paymentValue,
                CustomerId = customerId,
                OrderDate = DateTime.UtcNow,
            });

        }

        private async Task<Order> InsertOrder(Order order)
        {
            await _ctx.Orders.AddAsync(order);
            await _ctx.SaveChangesAsync();
            return order; 
        }
    }
}
