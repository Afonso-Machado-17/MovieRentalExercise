using Microsoft.EntityFrameworkCore;
using MovieRental.Data;

namespace MovieRental.Rental
{
	public class RentalFeatures : IRentalFeatures
	{
		private readonly MovieRentalDbContext _movieRentalDb;
		public RentalFeatures(MovieRentalDbContext movieRentalDb)
		{
			_movieRentalDb = movieRentalDb;
		}

		//TODO: make me async :( - done :)
		async public Task<Rental> Save(Rental rental)
		{
			await _movieRentalDb.Rentals.AddAsync(rental);
			await _movieRentalDb.SaveChangesAsync();
			return rental;
		}

		//TODO: finish this method and create an endpoint for it - done
		public async Task<IEnumerable<Rental>> GetRentalsByCustomerName(string customerName)
		{
             return await _movieRentalDb.Rentals
							.Include(r => r.Customer)
							.Where(r => r.Customer.CustomerName.Equals(customerName, StringComparison.CurrentCultureIgnoreCase))
							.ToListAsync();
        }
	}
}