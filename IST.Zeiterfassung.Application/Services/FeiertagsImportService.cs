using System.Net.Http;
using System.Text.Json;
using IST.Zeiterfassung.Application.DTOs.Feiertage;
using IST.Zeiterfassung.Application.Interfaces;
using IST.Zeiterfassung.Domain.Entities;

namespace IST.Zeiterfassung.Application.Services
{
    public class FeiertagsImportService
    {
        private readonly IFeiertagRepository _repository;
        private readonly HttpClient _httpClient = new();

        public FeiertagsImportService(IFeiertagRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Feiertag>> LadeUndSpeichereFeiertageAsync(int jahr, string countryCode = "AT")
        {
            var url = $"https://date.nager.at/api/v3/PublicHolidays/{jahr}/{countryCode}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var externFeiertage = JsonSerializer.Deserialize<List<FeiertagNagerDTO>>(json);

            if (externFeiertage is null || !externFeiertage.Any())
                throw new Exception("Keine Feiertage empfangen oder ungültige Antwort.");

            var liste = externFeiertage.Select(f => new Feiertag
            {
                Datum = f.date,
                Name = f.localName,
                RegionCode = f.countryCode,
                IstArbeitsfrei = f.type?.ToLowerInvariant() == "public",
                Kommentar = f.name // englischer Name als Kommentar
            }).ToList();

            await _repository.SaveAllAsync(liste);
            return liste;
        }
    }
}
