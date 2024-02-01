using HackerNews.Service;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HackerNewsController : ControllerBase
    {
        private readonly IHackerNews _hackerNewsService;
        public HackerNewsController(IHackerNews hackerNews)
        {
            _hackerNewsService = hackerNews;
        }

        [HttpGet("best-stories")]
        public async Task<IActionResult> GetBestStories(int count = 10)
        {
            try
            {
                var bestStoryIds = await _hackerNewsService.GetBestStoryIdsAsync();

                // Fetch details for the specified number of best stories
                var bestStories = await Task.WhenAll(bestStoryIds.Take(count).Select(id => _hackerNewsService.GetStoryDetailsAsync(id)));

                // Order by score in descending order
                bestStories = bestStories.OrderByDescending(s => s.Score).ToArray();

                // Convert to the specified response format
                var formattedResponse = bestStories.Select(story => new
                {
                    title = story.Title,
                    uri = story.Url,
                    postedBy = story.By,
                    time = DateTimeOffset.FromUnixTimeSeconds(story.Time).ToString(),
                    score = story.Score,
                    commentCount = story.Descendants
                }).ToList();

                return Ok(formattedResponse);
            }
            catch (Exception ex)
            {
                // Log the exception and return an error response
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
