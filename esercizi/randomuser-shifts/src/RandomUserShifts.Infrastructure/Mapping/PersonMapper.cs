using RandomUserShifts.Domain.Abstractions;
using RandomUserShifts.Domain.Entities;

namespace RandomUserShifts.Infrastructure.Mapping;

public static class PersonMapper
{
    public static Person Map(PersonDto dto)
    {
        // TODO[1]: mappa first+last -> FullName, email -> Email, nat -> Nationality, login.uuid -> Id (Guid).
        return new Person(Guid.NewGuid(), "TODO", dto.Email, dto.Nationality, 1);
    }
}
