using Microsoft.EntityFrameworkCore;
using ProvaPub.Repository;
using ProvaPub.Services;
using ProvaPub.Services.Customers;
using ProvaPub.Services.Orders;
using ProvaPub.Services.PaymentMethods;
using ProvaPub.Services.Products;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRandomService, RandomService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddScoped<IPaymentMethod, PixPaymentMethod>();
builder.Services.AddScoped<IPaymentMethod, CreditCardPaymentMethod>();
builder.Services.AddScoped<IPaymentMethod, PaypalPaymentMethod>();

builder.Services.AddDbContext<TestDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("ctx")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
