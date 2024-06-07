using Application;
using Application.Guest;
using Application.Guest.DTO;
using Application.Guest.Requests;
using AutoFixture;
using Domain.Entities;
using Domain.Ports;
using Moq;

namespace ApplicationTests
{

    public class Tests
    {
        GuestManager _guestManager;
        Mock<IGuestRepository> _mockRepo;
        Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _mockRepo = new Mock<IGuestRepository>();
            _guestManager = new GuestManager(_mockRepo.Object);
        }

        [Test]
        public async Task HappyPathWithoutAutoFixure()
        {
            var guestDto = new GuestDto
            {
                Name = "Fulano",
                Surname = "Ciclano",
                Email = "abc@gmail.com",
                IdNumber = "abca",
                IdTypeCode = 1
            };

            int expectedId = 222;

            var request = new CreateGuestRequest()
            {
                Data = guestDto,
            };

            _mockRepo.Setup(x => x.Create(It.IsAny<Guest>())).Returns(Task.FromResult(expectedId));

            _guestManager = new GuestManager(_mockRepo.Object);

            var res = await _guestManager.CreateGuest(request);

            Assert.IsNotNull(res);
            Assert.True(res.Success);
            Assert.AreEqual(res.Data.Id, expectedId);
            Assert.AreEqual(res.Data.Name, guestDto.Name);
        }

        [Test]
        public async Task HappyPathWithAutoFixure()
        {
            var guestDto = _fixture.Create<GuestDto>();

            int expectedId = guestDto.Id;

            var request = new CreateGuestRequest()
            {
                Data = guestDto,
            };

            _mockRepo.Setup(x => x.Create(It.IsAny<Guest>())).Returns(Task.FromResult(expectedId));

            _guestManager = new GuestManager(_mockRepo.Object);

            var res = await _guestManager.CreateGuest(request);

            Assert.IsNotNull(res);
            Assert.True(res.Success);
            Assert.AreEqual(res.Data.Id, expectedId);
            Assert.AreEqual(res.Data.Name, guestDto.Name);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("a")]
        [TestCase("ab")]
        [TestCase("abc")]
        public async Task Should_Return_InvalidPersonDocumentIdException_WhenDocsAreInvalid(string docNumber)
        {
            var guestDto = _fixture.Build<GuestDto>()
                           .With(x => x.IdNumber, docNumber)
                           .Create();

            int expectedId = guestDto.Id;

            var request = new CreateGuestRequest()
            {
                Data = guestDto,
            };

            _mockRepo.Setup(x => x.Create(It.IsAny<Guest>())).Returns(Task.FromResult(expectedId));

             _guestManager = new GuestManager(_mockRepo.Object);

            var res = await _guestManager.CreateGuest(request);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.INVALID_PERSON_ID);
            Assert.AreEqual(res.Message, "The ID passed is not valid");
        }

        [TestCase("", "", "")]
        [TestCase(null,null,null)]
        public async Task Should_Return_MissingRequiredInformation_WhenNameOrSurnameOrEmailAreInvalid(string name, string surname, string email)
        {
            var guestDto = _fixture.Build<GuestDto>()
                           .With(x => x.Name, name)
                           .With(x => x.Surname, surname)
                           .With(x => x.Email, email)
                           .Create();

            int expectedId = guestDto.Id;

            var request = new CreateGuestRequest()
            {
                Data = guestDto,
            };

            _mockRepo.Setup(x => x.Create(It.IsAny<Guest>())).Returns(Task.FromResult(expectedId));

            _guestManager = new GuestManager(_mockRepo.Object);

            var res = await _guestManager.CreateGuest(request);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.MISSING_REQUIRED_INFORMATION);
            Assert.AreEqual(res.Message, "Missing required information passed");
        }

        [TestCase("b@b.com")]
        public async Task Should_Return_InvalidEmailException_WhenDocsAreInvalid(string email)
        {
            var guestDto = _fixture.Build<GuestDto>()
                           .With(x => x.Email, email)
                           .Create();

            int expectedId = guestDto.Id;

            var request = new CreateGuestRequest()
            {
                Data = guestDto,
            };

            _mockRepo.Setup(x => x.Create(It.IsAny<Guest>())).Returns(Task.FromResult(expectedId));

            _guestManager = new GuestManager(_mockRepo.Object);

            var res = await _guestManager.CreateGuest(request);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.INVALID_EMAIL);
            Assert.AreEqual(res.Message, "The given email is not valid");
        }

        [Test]
        public async Task Should_Return_GuestNotFound_When_GuestDoesntExist()
        {
            var fakeId = _fixture.Create<int>();

            _mockRepo.Setup(x => x.Get(fakeId)).Returns(Task.FromResult<Guest?>(null));

            _guestManager = new GuestManager(_mockRepo.Object);

            var res = await _guestManager.GetGuest(fakeId);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.GUEST_NOT_FOUND);
            Assert.AreEqual(res.Message, "No Guest record was found with the given Id");
        }

        [Test]
        public async Task Should_Return_Guest_Success()
        {
            var guest = _fixture.Create<Guest>();

            int fakeId = guest.Id;

            _mockRepo.Setup(x => x.Get(fakeId)).Returns(Task.FromResult((Guest?)guest));

            _guestManager = new GuestManager(_mockRepo.Object);

            var res = await _guestManager.GetGuest(fakeId);

            Assert.IsNotNull(res);
            Assert.True(res.Success);
            Assert.AreEqual(res.Data.Id, guest.Id);
            Assert.AreEqual(res.Data.Name, guest.Name);
        }
    }
}