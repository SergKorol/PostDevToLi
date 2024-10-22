using Microsoft.Extensions.DependencyInjection;
using PostDevToLi.Services;

namespace PostDevToLi;

internal static class Program
{

    private static async Task Main(string[] args)
    {
        var apiKey = GetArgumentValue(args, "--api-key");
        var accessToken = GetArgumentValue(args, "--access-token");


        var serviceProvider = new ServiceCollection()
            .AddHttpClient()
            .AddSingleton<ArticleService>()
            .BuildServiceProvider();

        var articleService = serviceProvider.GetRequiredService<ArticleService>();

        await articleService.GetAndShareArticlesAsync(apiKey, accessToken);
        
    }
    
    private static string? GetArgumentValue(string[] args, string key)
    {
        return (from arg in args where arg.StartsWith(key) select arg.Split('=')[1].Trim('\'')).FirstOrDefault();
    }
    
}