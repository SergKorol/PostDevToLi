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
            .AddDbContext<ArticleDbContext>(options =>
                options.UseSqlite($"Data Source=../../../posted_articles{DateTime.Now}.db"))
            .AddScoped<ArticleService>()
            .BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ArticleDbContext>();
        await dbContext.Database.MigrateAsync();
        var articleService = scope.ServiceProvider.GetRequiredService<ArticleService>();

        TryParse(hoursAgo, out var hoursNumber);
        await articleService.GetAndShareArticlesAsync(apiKey, accessToken, hoursNumber, dbContext);
    }
    
    private static string? GetArgumentValue(string[] args, string key)
    {
        return (from arg in args where arg.StartsWith(key) select arg.Split('=')[1].Trim('\'')).FirstOrDefault();
    }
}