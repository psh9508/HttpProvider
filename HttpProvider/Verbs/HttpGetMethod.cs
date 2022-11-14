using HttpProvider.Bases;
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
    public class HttpGetMethod : HttpProviderBase, IHttpGetMethod
    {
        public async Task<(bool IsSuccess, TResult? Body)> GetAsync<TResult>(string uri)
        {
            var response = await _httpClient.GetAsync(uri);

            var debug = await response.Content.ReadAsStringAsync();

            return response.IsSuccessStatusCode ? (true, await response.Content.ReadFromJsonAsync<TResult>())
                                                : (false, default(TResult));
        }
    }
}
