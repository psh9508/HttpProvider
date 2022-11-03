using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace HttpProvider.Verbs
{
    public interface IHttpGetMethod
    {
        Task<(bool IsSuccess, TResult? Body)> GetAsync<TResult>(string uri);
    }
    public class HttpGetMethod : IHttpGetMethod
    {
        private readonly HttpClient _httpClient;

        public HttpGetMethod(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(bool IsSuccess, TResult? Body)> GetAsync<TResult>(string uri)
        {
            var response = await _httpClient.GetAsync(uri);

            return response.IsSuccessStatusCode ? (true, await response.Content.ReadFromJsonAsync<TResult>())
                                                : (false, default(TResult));
        }
    }
}
