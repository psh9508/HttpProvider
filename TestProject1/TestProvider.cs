using HttpProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    internal class TestProvider
    {
        private readonly IHttpHeader _httpProvider;

        public TestProvider()
        {
            // Injection 객체 넣어줘야 함. 좋은 설계가 맞는 것인가?
            //_httpProvider = new HttpProvider.HttpProvider();
        }
    }
}
