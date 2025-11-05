using System.Net.Http.Json;
using RandomUserShifts.Domain.Abstractions;
using RandomUserShifts.Infrastructure.Http.Models;

namespace RandomUserShifts.Infrastructure.Http;

public sealed class RandomUserHttpClient(HttpClient httpClient) : IRandomUserClient
{
    private static readonly Uri BaseUri = new("https://randomuser.me/");

    public async Task<IReadOnlyList<PersonDto>> GetRandomUsersAsync(int count, CancellationToken cancellationToken = default)
    {
        var requestUri = new Uri(BaseUri, $"api/?results={count}");
        var response = await httpClient.GetFromJsonAsync<RandomUserResponse>(requestUri, cancellationToken);
        if (response?.Results is null)
        {
            return Array.Empty<PersonDto>();
        }

        return response.Results.Select(r => new PersonDto(
            Guid.TryParse(r.Login.Uuid, out var uuid) ? uuid : Guid.NewGuid(),
            r.Name.First,
            r.Name.Last,
            r.Email,
            r.Nationality
        )).ToList();
    }
}
