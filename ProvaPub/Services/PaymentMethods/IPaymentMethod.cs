namespace ProvaPub.Services.PaymentMethods
{
    public interface IPaymentMethod
    {
        string Name { get; }
        void ProcessPayment(decimal paymentValue, int customerId);
    }
}