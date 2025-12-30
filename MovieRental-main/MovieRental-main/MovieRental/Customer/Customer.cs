using System.ComponentModel.DataAnnotations;

namespace MovieRental.Customer
{
    // Note - Wasn't sure if more fields were required from a business perspective (ex: email, phone, address) so decided to focus only on the existing project
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public required string CustomerName { get; set; }
        public ICollection<Rental.Rental> Rentals { get; set; } = new List<Rental.Rental>();
    }
}
