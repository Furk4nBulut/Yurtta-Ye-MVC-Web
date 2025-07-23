using System;
using System.Net.Http.Json;
using YurttaYe.Core.Models.Entities;
using YurttaYe.Web;

namespace YurttaYe.Web.Services
{
    public class ApiService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl = "http://localhost:5107/api"; // Domain: https://yurttaye.com/api
        private bool _disposed = false;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = new Uri("http://localhost:5107/");
        }

        public async Task<List<City>> GetCities()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<City>>($"{_apiUrl}/City") ?? new List<City>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Şehirler yüklenemedi: {ex.Message}");
            }
        }

        public async Task<List<Menu>> GetMenus(int? cityId, string? mealType, string? date)
        {
            try
            {
                var query = new List<string>();
                if (cityId.HasValue) query.Add($"cityId={cityId}");
                if (!string.IsNullOrEmpty(mealType)) query.Add($"mealType={mealType}");
                if (!string.IsNullOrEmpty(date)) query.Add($"date={date}");
                var url = $"{_apiUrl}/Menu" + (query.Any() ? $"?{string.Join("&", query)}" : "");
                return await _httpClient.GetFromJsonAsync<List<Menu>>(url) ?? new List<Menu>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Menüler yüklenemedi: {ex.Message}");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _httpClient?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}