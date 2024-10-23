using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PostDevToLi.Context;
using PostDevToLi.Models.dev.to;
using PostDevToLi.Models.linkedin.com;

namespace PostDevToLi.Services;

public class ArticleService(IHttpClientFactory clientFactory, ILogger<ArticleService> logger, ArticleDbContext dbContext)
{
    // public async Task GetAndShareArticlesAsync(string? apiKey, string? accessToken, int hoursAgo)
    // {
    //     var articles = await FetchArticlesAsync(apiKey);
    //
    //     if (articles == null || articles.Length == 0)
    //     {
    //         logger.LogWarning("No articles found to share");
    //         return;
    //     }
    //
    //     var lastArticles = articles.Where(x => x.PublishedAt >= DateTime.UtcNow.AddHours(hoursAgo * -1)).ToList();
    //
    //     if (lastArticles.Count != 0)
    //     {
    //         await ShareArticlesOnLinkedInAsync(lastArticles, accessToken);
    //     }
    //     else
    //     {
    //         logger.LogInformation("No new articles from the past {HoursAgo} days", hoursAgo);
    //     }
    // }
    public async Task GetAndShareArticlesAsync(string? apiKey, string? accessToken, int hoursAgo)
    {
        var articles = await FetchArticlesAsync(apiKey);

        if (articles == null || articles.Length == 0)
        {
            logger.LogWarning("No articles found to share");
            return;
        }

        var lastArticles = articles
            .Where(x => x.PublishedAt >= DateTime.UtcNow.AddHours(hoursAgo * -1))
            .ToList();

        if (!lastArticles.Any())
        {
            logger.LogInformation("No new articles from the past {HoursAgo} hours", hoursAgo);
            return;
        }

        foreach (var article in lastArticles)
        {
            bool isPosted = await dbContext.PostedArticles.AnyAsync(a => a.Url == article.Url);

            if (isPosted)
            {
                logger.LogInformation("Article '{Title}' has already been shared", article.Title);
                continue;
            }

            bool success = await ShareArticlesOnLinkedInAsync(article, accessToken);

            if (!success) continue;
            dbContext.PostedArticles.Add(new PostedArticle
            {
                Title = article.Title,
                Url = article.Url,
                PublishedAt = article.PublishedAt
            });

            await dbContext.SaveChangesAsync();
        }
    }

    private async Task<Article[]?> FetchArticlesAsync(string? apiKey)
    {
        var client = GetHttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.121 Safari/537.36");
        client.DefaultRequestHeaders.Add("api_key", apiKey);

        var response = await client.GetAsync("https://dev.to/api/articles/me/published");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Article[]>(content);
        }

        logger.LogError("Failed to fetch articles. Status Code: {ResponseStatusCode}", response.StatusCode);
        return null;
    }

    private async Task<bool> ShareArticlesOnLinkedInAsync(Article article, string? accessToken)
    {
        var client = GetHttpClient();
        var personUrn = await GetPersonUrnAsync(accessToken);

        if (string.IsNullOrEmpty(personUrn))
        {
            logger.LogError("Failed to retrieve LinkedIn user info");
            return false;
        }

        var requestMessage = CreateLinkedInPostRequest(accessToken);
        var shareRequest = CreateLinkedInPostRequestBody(article, personUrn);
            
        var jsonRequest = JsonSerializer.Serialize(shareRequest);
        var jsonContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        requestMessage.Content = jsonContent;

        var response = await client.SendAsync(requestMessage);
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var linkedInResponse = JsonSerializer.Deserialize<LinkedInResponse>(content, options);

            if (linkedInResponse?.IsSuccess == true)
            {
                logger.LogInformation("Post {LinkedInResponse} was successfully shared", linkedInResponse.Id);
                return true;
            }
        }
        else if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
        {
            logger.LogWarning("The article '{Title}' has already been shared", article.Title);
            return false;
        }
        else
        {
            logger.LogError("Failed to share article: {ArticleTitle}. Status Code: {ResponseStatusCode}", article.Title, response.StatusCode);
            return false;
        }
        return false;
    }

    private async Task<string> GetPersonUrnAsync(string? accessToken)
    {
        var client = clientFactory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        client.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");

        var response = await client.GetAsync("https://api.linkedin.com/v2/userinfo");

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Failed to fetch LinkedIn user info. Status Code: {ResponseStatusCode}", response.StatusCode);
            return string.Empty;
        }

        var content = await response.Content.ReadAsStringAsync();
        var userInfo = JsonSerializer.Deserialize<UserResponse>(content);

        return userInfo != null ? $"urn:li:person:{userInfo.Sub}" : string.Empty;
    }

    private LinkedInPost CreateLinkedInPostRequestBody(Article article, string personUrn)
    {
        var media = new Media
        {
            OriginalUrl = article.Url,
            Description = !string.IsNullOrEmpty(article.Description) ? new TextBlock { Text = article.Description } : null,
            Title = !string.IsNullOrEmpty(article.Title) ? new TextBlock { Text = article.Title } : null,
            Thumbnails = !string.IsNullOrEmpty(article.CoverImage)
                ? new[] { new Thumbnail { Url = article.CoverImage, Width = 128, Height = 72, AltText = article.Title } }
                : null
        };

        return new LinkedInPost
        {
            Author = personUrn,
            Visibility = new Visibility { VisibilityType = VisibilityType.Anyone },
            SpecificContent = new SpecificContent
            {
                ShareContent = new ShareContent
                {
                    ShareCommentary = new TextBlock { Text = "Check out this new article!" },
                    ShareMediaCategoryType = ShareMediaCategoryType.Article,
                    Media = new[] { media }
                }
            }
        };
    }

    private HttpRequestMessage CreateLinkedInPostRequest(string? accessToken)
    {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.linkedin.com/v2/ugcPosts");
        requestMessage.Headers.Add("Authorization", $"Bearer {accessToken}");
        requestMessage.Headers.Add("X-Restli-Protocol-Version", "2.0.0");

        return requestMessage;
    }

    private HttpClient GetHttpClient()
    {
        return clientFactory.CreateClient();
    }
}

