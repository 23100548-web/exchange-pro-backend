using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using UESAN.ExchangePro.CORE.Core.Interfaces;

namespace UESAN.ExchangePro.CORE.Core.Services
{
    public class TipoCambioService : ITipoCambioService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "TipoCambio";
        private static readonly string[] Codigos = { "USD", "EUR", "GBP", "JPY" };

        public TipoCambioService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<TipoCambioResponseDTO> GetTipoCambio()
        {
            if (_cache.TryGetValue(CacheKey, out TipoCambioResponseDTO cached))
                return cached!;

            var hoy = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var ayer = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd");

            var (ratesHoy, fechaReal) = await FetchRates(hoy);
            var (ratesAyer, _) = await FetchRates(ayer);

            var spread = 0.03m;
            var monedas = new List<MonedaTipoCambioDTO>();

            foreach (var codigo in Codigos)
            {
                if (!ratesHoy.TryGetValue(codigo, out var tasaHoyVal) || tasaHoyVal <= 0)
                    continue;

                var mid = Math.Round(1 / tasaHoyVal, 4);
                ratesAyer.TryGetValue(codigo, out var tasaAyerVal);
                var midAyer = tasaAyerVal > 0 ? Math.Round(1 / tasaAyerVal, 4) : mid;

                var direccion = mid > midAyer ? "sube" : mid < midAyer ? "baja" : "estable";

                monedas.Add(new MonedaTipoCambioDTO
                {
                    Codigo = codigo,
                    Mid = mid,
                    Compra = Math.Round(mid - spread, 3),
                    Venta = Math.Round(mid + spread, 3),
                    Direccion = direccion
                });
            }

            var dto = new TipoCambioResponseDTO
            {
                Monedas = monedas,
                Fecha = fechaReal
            };

            var cacheOpts = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(30));

            _cache.Set(CacheKey, dto, cacheOpts);

            return dto;
        }

        private async Task<(Dictionary<string, decimal> rates, string date)> FetchRates(string date)
        {
            try
            {
                var symbols = string.Join(",", Codigos);
                var url = $"https://api.frankfurter.app/{date}?from=PEN&to={symbols}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                var rates = new Dictionary<string, decimal>();
                foreach (var prop in doc.RootElement.GetProperty("rates").EnumerateObject())
                {
                    rates[prop.Name] = prop.Value.GetDecimal();
                }

                var fecha = doc.RootElement.GetProperty("date").GetString() ?? date;
                return (rates, fecha);
            }
            catch
            {
                var fallback = new Dictionary<string, decimal>
                {
                    { "USD", 0.2658m },
                    { "EUR", 0.2445m },
                    { "GBP", 0.2091m },
                    { "JPY", 41.67m }
                };
                return (fallback, date);
            }
        }
    }
}