using HttpProvider;
using HttpProvider.Verbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class SummonerInfo
    {
        public string Id { get; set; } = string.Empty;
        public string AccountId { get; set; } = string.Empty;
        public string Puuid { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int ProfileIconId { get; set; }
        public long RevisionDate { get; set; }
        public int SummonerLevel { get; set; }
    }

    internal class TestProvider
    {
        private const string USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36";
        private const string ACCEPT_LANGUAGE = "ko,en-US;q=0.9,en;q=0.8";
        private const string ACCEPT_CHARSET = "application/x-www-form-urlencoded; charset=UTF-8";
        private const string ORIGIN = "https://developer.riotgames.com";
        private readonly string X_RIOT_TOKEN;

        private readonly HttpProvider.HttpProvider _httpProvider;
        private static HttpClient _httpClient;

        public TestProvider()
        {
            _httpClient = new HttpClient();

            X_RIOT_TOKEN = ApiSecret.API_KEY;
            _httpClient.DefaultRequestHeaders.Add("X-Riot-Token", X_RIOT_TOKEN);

            // Injection 객체 넣어줘야 함. 좋은 설계가 맞는 것인가?
            _httpProvider = new HttpProvider.HttpProvider(new HttpPostMethod(_httpClient), new HttpGetMethod(_httpClient));
        }

        public async Task<SummonerInfo?> GetSummonerInfo(string summonerName)
        {
            var result = await _httpProvider.GetAsync<SummonerInfo>(@$"https://br1.api.riotgames.com/lol/summoner/v4/summoners/by-name/{summonerName}");

            return result;
        }
    }
}