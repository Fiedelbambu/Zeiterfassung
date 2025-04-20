using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Entities;
using System.Text.Json;

namespace IST.Zeiterfassung.Application.Services;

public class FeiertagsService : IFeiertagsService
{
    private readonly IFeiertagRepository _repo;
    private readonly HttpClient _http;

    public FeiertagsService(IHttpClientFactory httpFactory, IFeiertagRepository repo)
    {
        _http = httpFactory.CreateClient();
        _repo = repo;
    }

    public async Task<List<Feiertag>> GetFeiertageAsync(int jahr, string region = "AT")
    {
        var lokale = await _repo.GetAlleAsync(jahr, region);

        if (lokale.Any())
            return lokale;

        // Nager.Date als Fallback
        var url = $"https://date.nager.at/api/v3/PublicHolidays/{jahr}/{region}";
        var response = await _http.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return lokale;

        var json = await response.Content.ReadAsStringAsync();
        var daten = JsonSerializer.Deserialize<List<NagerFeiertag>>(json);

        var neueFeiertage = daten?.Select(f => new Feiertag
        {
            Id = Guid.NewGuid(),
            Datum = f.Date,
            Name = f.LocalName,
            RegionCode = region
        }).ToList() ?? new();

        foreach (var f in neueFeiertage)
            await _repo.AddAsync(f);

        return neueFeiertage;
    }

    public async Task<bool> IstFeiertagAsync(DateTime datum, string region = "AT")
    {
        var bekannte = await _repo.ExistsAsync(datum, region);
        if (bekannte)
            return true;

        var liste = await GetFeiertageAsync(datum.Year, region);
        return liste.Any(f => f.Datum.Date == datum.Date);
    }

    private class NagerFeiertag
    {
        public DateTime Date { get; set; }
        public string LocalName { get; set; } = "";
    }
}
