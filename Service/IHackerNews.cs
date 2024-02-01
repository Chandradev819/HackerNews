using HackerNews.Model;
namespace HackerNews.Service
{
    public interface IHackerNews
    {
        Task<int[]> GetBestStoryIdsAsync();
        Task<StoryDetails> GetStoryDetailsAsync(int storyId);
    }
}
