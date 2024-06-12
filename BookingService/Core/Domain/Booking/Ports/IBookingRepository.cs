namespace Domain.Booking.Ports
{
    public interface IBookingRepository
    {
        Task<Guest.Entities.Booking> Get(int id);
        Task<Guest.Entities.Booking> CreateBooking(Guest.Entities.Booking booking);
    }
}
