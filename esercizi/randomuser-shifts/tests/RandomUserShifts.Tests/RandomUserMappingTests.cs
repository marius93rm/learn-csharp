using RandomUserShifts.Domain.Abstractions;
using RandomUserShifts.Infrastructure.Mapping;
using Xunit;

namespace RandomUserShifts.Tests;

public sealed class RandomUserMappingTests
{
    [Fact]
    public void PersonMapper_ComposesFullNameAndMapsFields()
    {
        // Arrange: DTO mimicking the RandomUser API contract.
        var expectedId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeffffffff");
        var dto = new PersonDto(expectedId, "Ada", "Lovelace", "ada@example.com", "GB");

        // Act
        var person = PersonMapper.Map(dto);

        // Assert: test rosso finché TODO[1] non è implementato.
        Assert.Equal(expectedId, person.Id);
        Assert.Equal("Ada Lovelace", person.FullName);
        Assert.Equal("ada@example.com", person.Email);
        Assert.Equal("GB", person.Nationality);
    }
}
