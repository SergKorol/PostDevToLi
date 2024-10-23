using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PostDevToLi.Context;

public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ArticleDbContext>
{
    public ArticleDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ArticleDbContext>();
        builder.UseSqlite("Data Source=posted_articles.db");
    
        return new ArticleDbContext(builder.Options);
    }
}