namespace ProvaPub.Services.PaymentMethods
{
    public class CreditCardPaymentMethod : IPaymentMethod
    {
        public string Name => "creditcard";

        public void ProcessPayment(decimal paymentValue, int customerId)
        {
            // Faz pagamento com Cartão de Crédito...
        }
    }
}