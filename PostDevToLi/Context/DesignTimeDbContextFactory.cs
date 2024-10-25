using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PostDevToLi.Context;

public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ArticleDbContext>
{
    public ArticleDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ArticleDbContext>();
        var projectRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".."));
        var dbPath = Path.Combine(projectRoot, "posted_articles.db");
        builder.UseSqlite($"Data Source={dbPath}");
    
        return new ArticleDbContext(builder.Options);
    }
}