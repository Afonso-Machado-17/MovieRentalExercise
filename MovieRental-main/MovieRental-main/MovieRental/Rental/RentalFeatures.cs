using Microsoft.EntityFrameworkCore;
using MovieRental.Data;
using MovieRental.PaymentProviders;

namespace MovieRental.Rental
{
	public class RentalFeatures : IRentalFeatures
	{
		private readonly MovieRentalDbContext _movieRentalDb;
        private readonly IEnumerable<IPaymentProvider> _paymentProviders;

        public RentalFeatures(MovieRentalDbContext movieRentalDb, IEnumerable<IPaymentProvider> paymentProviders)
		{
			_movieRentalDb = movieRentalDb;
            _paymentProviders = paymentProviders;
        }

		//TODO: make me async :( - done :)
		async public Task<Rental> Save(Rental rental)
		{
            await ProcessPayment(rental);

            await _movieRentalDb.Rentals.AddAsync(rental);
			await _movieRentalDb.SaveChangesAsync();
			return rental;
		}

		//TODO: finish this method and create an endpoint for it - done
		public async Task<IEnumerable<Rental>> GetRentalsByCustomerName(string customerName)
		{
             return await _movieRentalDb.Rentals
							.Include(r => r.Customer)
                            .Include(r => r.Movie)
                            .Where(r => r.Customer.CustomerName.ToLower() == customerName.ToLower())
                            .ToListAsync();
        }

        private async Task ProcessPayment(Rental rental)
        {
            var provider = _paymentProviders.FirstOrDefault(p =>
                p.PaymentMethod.Equals(rental.PaymentMethod, StringComparison.OrdinalIgnoreCase));

            if (provider == null)
                throw new InvalidOperationException($"Unsupported payment method: {rental.PaymentMethod}");

            var price = rental.DaysRented * 5.0; // Random price placeholder

            var paymentSuccessful = await provider.Pay(price);

            if (!paymentSuccessful)
                throw new InvalidOperationException("Payment failed. Rental was not created.");
        }
    }
}