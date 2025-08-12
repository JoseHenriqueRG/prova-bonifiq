using ProvaPub.Models;

namespace ProvaPub.Services.Customers
{
    public interface ICustomerService
    {
        Task<PagedList<Customer>> ListCustomers(int page);
        Task<bool> CanPurchase(int customerId, decimal purchaseValue);
    }
}