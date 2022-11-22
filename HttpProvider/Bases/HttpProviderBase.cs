using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpProvider.Bases
{
    public abstract class HttpProviderBase
    {
        protected static HttpClient _httpClient { get; private set; }

        static HttpProviderBase()
        {
            _httpClient = new HttpClient();
        }
    }
}
