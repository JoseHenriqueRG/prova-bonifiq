using Microsoft.EntityFrameworkCore;
using Moq;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Services.Customers;

namespace ProvaPubTests
{
    public class CustomerServiceTests
    {
        private readonly CustomerService _customerService;
        private readonly TestDbContext _context;

        public CustomerServiceTests()
        {
            _context = GetInMemoryDbContext();
            _customerService = new CustomerService(_context);
        }

        private TestDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
                .Options;

            return new TestDbContext(options);
        }

        [Fact]
        public async Task ListCustomers_ReturnsPagedListOfCustomers()
        {
            // Arrange: adiciona clientes reais no contexto in-memory
            _context.Customers.AddRange(
                new Customer { Id = 1, Name = "Test Customer 1" },
                new Customer { Id = 2, Name = "Test Customer 2" }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _customerService.ListCustomers(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalCount);
            Assert.False(result.HasNext);
            Assert.Equal(2, result.Items.Count);
        }

        [Fact]
        public async Task CanPurchase_CustomerIdIsZero_ThrowsArgumentOutOfRangeException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _customerService.CanPurchase(0, 10));
        }

        [Fact]
        public async Task CanPurchase_PurchaseValueIsZero_ThrowsArgumentOutOfRangeException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _customerService.CanPurchase(1, 0));
        }

        [Fact]
        public async Task CanPurchase_CustomerNotFound_ThrowsInvalidOperationException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _customerService.CanPurchase(1, 10));
        }

        [Fact]
        public async Task CanPurchase_AlreadyBoughtThisMonth_ReturnsFalse()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer" };
            var order = new Order
            {
                CustomerId = 1,
                OrderDate = DateTime.UtcNow.AddDays(-1)
            };

            _context.Customers.Add(customer);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Act
            var result = await _customerService.CanPurchase(1, 10);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public async Task CanPurchase_FirstPurchaseOver100_ReturnsFalse()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer" };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _customerService.CanPurchase(1, 101);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public async Task CanPurchase_OutsideBusinessHours_ReturnsFalse()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer" };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _customerService.CanPurchase(1, 10);

            // Assert
            var now = DateTime.UtcNow;
            bool outsideBusinessHours = now.Hour < 8 || now.Hour > 18 || now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday;

            if (outsideBusinessHours)
            {
                Assert.False(result);
            }
            else
            {
                Assert.True(result);
            }
        }

        [Fact]
        public async Task CanPurchase_ValidPurchase_ReturnsTrue()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer" };
            _context.Customers.Add(customer);
            var order = new Order { CustomerId = 1, Value = 99 };
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            // Act
            var result = await _customerService.CanPurchase(1, 99);

            // Assert
            var now = DateTime.UtcNow;
            bool outsideBusinessHours = now.Hour < 8 || now.Hour > 18 || now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday;

            if (outsideBusinessHours)
            {
                Assert.False(result);
            }
            else
            {
                Assert.True(result);
            }
        }

    }
}