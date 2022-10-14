using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HttpProvider.Extensions
{
    public static class ObjectExtension
    {
        public static Dictionary<string, string> ToDictionary<T>(this T src)
        {
            var jsonBody = JsonSerializer.Serialize(src);

            return JsonSerializer.Deserialize<Dictionary<string, string>>(jsonBody);
        }

        public static Dictionary<string, string> ToDictionary<T>(string src)
        {
            return JsonSerializer.Deserialize<Dictionary<string, string>>(src);
        }
    }
}
