using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpProvider.Bases
{
    public abstract class HttpProviderBase
    {
        public static HttpClient _httpClient;

        static HttpProviderBase()
        {
            _httpClient = new HttpClient();
        }
    }
}
