using HttpProvider.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpProvider
{
    public interface IHttpHeader
    {
        bool HasHeader(string name);
        bool RemoveHeader(string name);
        bool TryGetHeaderValue(string name, out string value);
        void AddHeader(string name, string value, bool isOverried = false);
    }

    public class HttpProvider : IHttpHeader
    {
        protected static readonly HttpClient _httpClient = new HttpClient();

        public HttpProvider()
        {
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri("https://localhost:5001/");
                _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        public bool HasHeader(string name)
        {
            return _httpClient.DefaultRequestHeaders.Contains(name);
        }

        public bool RemoveHeader(string name)
        {
            if (HasHeader(name))
            {
                _httpClient.DefaultRequestHeaders.Remove(name);
                return true;
            }

            return false;
        }

        public bool TryGetHeaderValue(string name, out string value)
        {
            value = string.Empty;

            if (HasHeader(name))
            {
                value = _httpClient.DefaultRequestHeaders.GetValues(name).Single();
                return true;
            }

            return false;
        }

        public void AddHeader(string name, string value, bool isOverried = false)
        {
            if (isOverried)
            {
                if (_httpClient.DefaultRequestHeaders.Contains(name))
                    _httpClient.DefaultRequestHeaders.Remove(name);

                _httpClient.DefaultRequestHeaders.Add(name, value);
            }
            else
            {
                if (_httpClient.DefaultRequestHeaders.Contains(name))
                    return;

                _httpClient.DefaultRequestHeaders.Add(name, value);
            }
        }

        public async Task<(bool IsSuccess, TResult Body)> GetAsync<TResult>(string uri)
        {
            try
            {
                var result = await _httpClient.GetAsync(uri);

                var debug = await result.Content.ReadAsStringAsync();

                return (true, await result.Content.ReadFromJsonAsync<TResult>());
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
