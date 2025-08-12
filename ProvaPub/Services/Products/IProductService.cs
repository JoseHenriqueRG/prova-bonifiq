using ProvaPub.Models;

namespace ProvaPub.Services.Products
{
    public interface IProductService
    {
        Task<PagedList<Product>> ListProducts(int page);
    }
}