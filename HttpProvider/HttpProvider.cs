using HttpProvider.Extensions;
using HttpProvider.Verbs;
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

        private readonly IHttpPostMethod _post;
        private readonly IHttpGetMethod _get;

        public HttpProvider(IHttpPostMethod postMethod, IHttpGetMethod getMethod)
        {
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri("https://localhost:5001/");
                _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }
            
            _post = postMethod;
            _get = getMethod;
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

        public async Task<T?> GetAsync<T>(string url)
        {
            var result = await _get.GetAsync<T>(url);

            return result.IsSuccess ? result.Body : default(T);
        }

        public async Task<(bool, TResult?)> PostAsync<TResult, TBody>(string url, TBody body)
        {
            try
            {
                //var response = await postAsync();
                var response = await _httpClient.PostAsJsonAsync(url, body);

                var debug = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode == false)
                    return (false, default(TResult));

                return (true, await response.Content.ReadFromJsonAsync<TResult>());
            }
            catch (NotSupportedException) // When content type is not valid
            {
                return (false, default(TResult));
            }
            catch (JsonException ex) // Invalid JSON
            {
                return (false, default(TResult));
            }
        }
    }
}
