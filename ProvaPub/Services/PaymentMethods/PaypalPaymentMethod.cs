namespace ProvaPub.Services.PaymentMethods
{
    public class PaypalPaymentMethod : IPaymentMethod
    {
        public string Name => "paypal";

        public void ProcessPayment(decimal paymentValue, int customerId)
        {
            // Faz pagamento com PayPal...
        }
    }
}