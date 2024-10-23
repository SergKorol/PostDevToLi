using Microsoft.EntityFrameworkCore;
using PostDevToLi.Models.linkedin.com;

namespace PostDevToLi.Context;

public class ArticleDbContext(DbContextOptions<ArticleDbContext> options) : DbContext(options)
{
    public DbSet<PostedArticle> PostedArticles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PostedArticle>().HasKey(a => a.Id);
    }
}