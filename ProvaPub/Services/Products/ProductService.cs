using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services.Products
{
    public class ProductService : IProductService
    {
        TestDbContext _ctx;

        public ProductService(TestDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<PagedList<Product>> ListProducts(int page)
        {
            int pageSize = 10;
            var totalCount = await _ctx.Products.CountAsync();
            var products = await _ctx.Products.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<Product>()
            {
                Items = products,
                TotalCount = totalCount,
                HasNext = page * pageSize < totalCount
            };
        }

    }
}
