using Domain.Booking.Ports;
using Microsoft.EntityFrameworkCore;

namespace Data.Booking
{
    public class BookingRepository : IBookingRepository
    {
        private readonly HotelDbContext _dbContext;
        public BookingRepository(HotelDbContext hotelDbContext)
        {
            _dbContext = hotelDbContext;
        }
        public async Task<Domain.Guest.Entities.Booking> CreateBooking(Domain.Guest.Entities.Booking booking)
        {
            _dbContext.Bookings.Add(booking);
            await _dbContext.SaveChangesAsync();
            return booking;
        }

        public Task<Domain.Guest.Entities.Booking> Get(int id)
        {
            return _dbContext.Bookings
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .Where(x => x.Id == id).FirstAsync();
        }
    }
}
