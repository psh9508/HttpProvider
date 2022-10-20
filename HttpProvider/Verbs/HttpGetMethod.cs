using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpProvider.Verbs
{
    public interface IHttpGetMethod
    {
        Task<(bool IsSuccess, TResult Body)> GetAsync<TResult>(string uri);
    }
    public class HttpGetMethod : IHttpGetMethod
    {
        public Task<(bool IsSuccess, TResult Body)> GetAsync<TResult>(string uri)
        {
            throw new NotImplementedException();
        }
    }
}
