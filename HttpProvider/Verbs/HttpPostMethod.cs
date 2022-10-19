using HttpProvider.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpProvider.Verbs
{
    public interface IHttpPostMethod
    {

    }

    public class HttpPostMethod : IHttpPostMethod
    {
        private readonly HttpClient _httpClient;

        public HttpPostMethod(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(bool IsSuccess, TResult Body)> PostAsync<TResult, TRequest>(string uri, TRequest body, string contentType = "application/json")
        {
            if (contentType == "application/x-www-form-urlencoded")
            {
                return await PostWithURLEncoding<TResult, TRequest>(uri, body);
            }
            else
            {
                return await PostWithJsonEncoding<TResult, TRequest>(uri, body);
            }
        }

        public async Task<(bool IsSuccess, TResult Body)> PostAsync<TResult, TRequest>(string uri, TRequest body)
        {
            return await DoPostBodyAsync<TResult>(async () =>
            {
                var debugBody = JsonSerializer.Serialize(body);

                return await _httpClient.PostAsJsonAsync(uri, body);
            });
        }

        private async Task<(bool IsSuccess, TResult Body)> PostWithJsonEncoding<TResult, TRequest>(string uri, TRequest body)
        {
            return await DoPostBodyAsync<TResult>(async () =>
            {
                var debug = JsonSerializer.Serialize(body);

                return await _httpClient.PostAsJsonAsync(uri, body);
            });
        }

        private async Task<(bool IsSuccess, TResult Body)> PostWithURLEncoding<TResult, TRequest>(string uri, TRequest body)
        {
            return await DoPostBodyAsync<TResult>(async () =>
            {
                var values = body.ToDictionary();

                using (var content = new FormUrlEncodedContent(values))
                {
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                    return await _httpClient.PostAsync(uri, content);
                }
            });
        }

        private async Task<(bool IsSuccess, TResult Body)> DoPostBodyAsync<TResult>(Func<Task<HttpResponseMessage>> postAsync)
        {
            try
            {
                var response = await postAsync();

                var debug = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode == false)
                    return (false, default(TResult));

                return (true, await response.Content.ReadFromJsonAsync<TResult>());
            }
            catch (Exception ex) when (ex is NotSupportedException || // When content type is not valid
                                       ex is JsonException            // Invalid JSON
                                       )
            {
                return (false, default(TResult));
            }
        }
    }
}
