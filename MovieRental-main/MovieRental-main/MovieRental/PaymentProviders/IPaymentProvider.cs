namespace MovieRental.PaymentProviders
{
    public interface IPaymentProvider
    {
        string PaymentMethod { get; }
        Task<bool> Pay(double price);
    }
}
