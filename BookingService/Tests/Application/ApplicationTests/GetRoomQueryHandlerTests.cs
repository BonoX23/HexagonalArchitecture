using Application.Room.Queries;
using AutoFixture;
using Domain.Room.Entities;
using Domain.Room.Ports;
using Moq;

namespace ApplicationTests
{
    public class GetRoomQueryHandlerTests
    {
        private GetRoomQueryHandler _getRoomQueryHandler;
        private Mock<IRoomRepository> _mockRepo;
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _mockRepo = new Mock<IRoomRepository>();
            _getRoomQueryHandler = new GetRoomQueryHandler(_mockRepo.Object);
        }

        [Test]
        public async Task Should_Return_Room()
        {
            var query = new GetRoomQuery { Id = 1 };

            var fakeRoom = new Room() { Id = 1 };
            _mockRepo.Setup(x => x.Get(query.Id)).Returns(Task.FromResult(fakeRoom));

            var handler = _getRoomQueryHandler;
            var res = await handler.Handle(query, CancellationToken.None);

            Assert.True(res.Success);
            Assert.NotNull(res.Data);
        }

        [Test]
        public async Task Should_Return_ProperError_Message_WhenRoom_NotFound()
        {
            var query = new GetRoomQuery { Id = 1 };
            var handler = _getRoomQueryHandler;
            var res = await handler.Handle(query, CancellationToken.None);

            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, Application.ErrorCodes.ROOM_NOT_FOUND);
            Assert.AreEqual(res.Message, "Could not find a Room with the given Id");
        }
    }
}
