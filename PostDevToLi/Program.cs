using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PostDevToLi.Context;
using PostDevToLi.Services;
using static System.Int16;

namespace PostDevToLi;

internal static class Program
{

    private static async Task Main(string[] args)
    {
        var apiKey = GetArgumentValue(args, "--api-key");
        var accessToken = GetArgumentValue(args, "--access-token");
        var hoursAgo = GetArgumentValue(args, "--ago");


        var serviceProvider = new ServiceCollection()
            .AddHttpClient()
            .AddSingleton<ArticleService>()
            .AddDbContext<ArticleDbContext>(options =>
                options.UseSqlite("Data Source=posted_articles.db"))
            .BuildServiceProvider();

        var articleService = serviceProvider.GetRequiredService<ArticleService>();

        TryParse(hoursAgo, out var hoursNumber);
        await articleService.GetAndShareArticlesAsync(apiKey, accessToken, hoursNumber);
        
    }
    
    private static string? GetArgumentValue(string[] args, string key)
    {
        return (from arg in args where arg.StartsWith(key) select arg.Split('=')[1].Trim('\'')).FirstOrDefault();
    }
}