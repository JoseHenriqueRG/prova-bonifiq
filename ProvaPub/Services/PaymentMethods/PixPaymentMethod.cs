namespace ProvaPub.Services.PaymentMethods
{
    public class PixPaymentMethod : IPaymentMethod
    {
        public string Name => "pix";

        public void ProcessPayment(decimal paymentValue, int customerId)
        {
            // Faz pagamento Pix...
        }
    }
}