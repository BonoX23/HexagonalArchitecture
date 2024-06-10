using Domain.Guest.ValueObjects;
using Domain.Room.Exceptions;
using Domain.Room.Ports;

namespace Domain.Room.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public bool InMaintenance { get; set; }
        public Price Price { get; set; }
        public bool IsAvailable
        {
            get
            {
                if (InMaintenance || HasGuest)
                {
                    return false;
                }
                return true;
            }
        }
        public bool HasGuest
        {
            // Verificar se existem booking abertos para esta room
            get
            {
                return true;
            }
        }

        private void ValidateState()
        {
            if (string.IsNullOrEmpty(Name))
            {
                throw new InvalidRoomDataException();
            }

            if (Price == null || Price.Value < 10)
            {
                throw new InvalidRoomPriceException();
            }
        }

        public async Task Save(IRoomRepository roomRepository)
        {
            ValidateState();

            if (Id == 0)
            {
                Id = await roomRepository.Create(this);
            }
            else
            {
            }
        }
    }
}
