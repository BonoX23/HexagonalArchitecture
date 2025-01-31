using Domain.Guest.Entities;
using Domain.Guest.Enums;
using Action = Domain.Guest.Enums.Action;

namespace DomainTests.Bookings
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldAlwaysStartWithCreatedStatus()
        {
            var booking = new Booking();

            Assert.AreEqual(booking.Status, Status.Created);
        }

        [Test]
        public void ShouldSetStatusToPaidWhenPayingForBookingWithCreatedStatus()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Pay);

            Assert.AreEqual(booking.Status, Status.Paid);
        }

        [Test]
        public void ShouldSetStatusToCancelWhenCancelingForBookingWithCreatedStatus()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Cancel);

            Assert.AreEqual(booking.Status, Status.Canceled);
        }

        [Test]
        public void ShouldSetStatusToFinishedWhenFinishingPaidBooking()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Pay);
            booking.ChangeState(Action.Finish);

            Assert.AreEqual(booking.Status, Status.Finished);
        }

        [Test]
        public void ShouldSetStatusToRefoundedWhenRefoundingPaidBooking()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Pay);
            booking.ChangeState(Action.Refound);

            Assert.AreEqual(booking.Status, Status.Refounded);
        }

        [Test]
        public void ShouldSetStatusToCreatedWhenReopeningCanceledBooking()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Cancel);
            booking.ChangeState(Action.Reopen);

            Assert.AreEqual(booking.Status, Status.Created);
        }


        //Teste negativo
        [Test]
        public void ShouldNotChangeStatusWhenRefoundingABookingWithCreatedStatus()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Refound);

            Assert.AreEqual(booking.Status, Status.Created);
        }

        //Teste negativo
        [Test]
        public void ShouldNotChangeStatusWhenRefoundingAFinishedBooking()
        {
            var booking = new Booking();

            booking.ChangeState(Action.Pay);
            booking.ChangeState(Action.Finish);
            booking.ChangeState(Action.Refound);

            Assert.AreEqual(booking.Status, Status.Finished);
        }
    }
}