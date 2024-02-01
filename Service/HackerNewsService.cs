using HackerNews.Model;
using Newtonsoft.Json;
using System.Net.Http;

namespace HackerNews.Service
{
    public class HackerNewsService : IHackerNews
    {
        private readonly HttpClient _httpClient;
        public HackerNewsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<int[]> GetBestStoryIdsAsync()
        {
            var response = await _httpClient.GetStringAsync("https://hacker-news.firebaseio.com/v0/beststories.json");
            return JsonConvert.DeserializeObject<int[]>(response!) ?? throw new Exception("Deserialization failed");
        }

        public async Task<StoryDetails> GetStoryDetailsAsync(int storyId)
        {
            var response = await _httpClient.GetStringAsync($"https://hacker-news.firebaseio.com/v0/item/{storyId}.json");
            return JsonConvert.DeserializeObject<StoryDetails>(response!) ?? throw new Exception("Deserialization failed");
        }
    }
}
