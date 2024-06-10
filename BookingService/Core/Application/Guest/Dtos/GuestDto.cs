using Entities = Domain.Guest.Entities;
using Domain.Guest.ValueObjects;
using Domain.Guest.Enums;

namespace Application.Guest.DTO
{
    public class GuestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string IdNumber { get; set; }
        public int IdTypeCode { get; set; }

        public static Entities.Guest MapToEntity(GuestDto guestDTO)
        {
            return new Entities.Guest
            {
                Id = guestDTO.Id,
                Name = guestDTO.Name,
                Surname = guestDTO.Surname,
                Email = guestDTO.Email,
                DocumentId = new PersonId
                {
                    IdNumber = guestDTO.IdNumber,
                    DocumentType = (DocumentType)guestDTO.IdTypeCode
                }
            };
        }

        public static GuestDto MapToDto(Domain.Guest.Entities.Guest guest)
        {
            return new GuestDto
            {
                Id = guest.Id,
                Email = guest.Email,
                IdNumber = guest.DocumentId.IdNumber,
                IdTypeCode = (int)guest.DocumentId.DocumentType,
                Name = guest.Name,
                Surname = guest.Surname,
            };
        }
    }
}