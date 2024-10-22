using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PostDevToLi.Models.dev.to;

public class Article
{
    [JsonPropertyName("type_of")]
    public required string TypeOf { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [Required]
    [JsonPropertyName("title")]
    public required string Title { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("published")]
    public bool Published { get; set; }

    [JsonPropertyName("published_at")]
    public DateTime PublishedAt { get; set; }

    [JsonPropertyName("slug")]
    public required string Slug { get; set; }

    [JsonPropertyName("path")]
    public required string Path { get; set; }

    [JsonPropertyName("url")]
    public required string Url { get; set; }

    [JsonPropertyName("comments_count")]
    public int CommentsCount { get; set; }

    [JsonPropertyName("public_reactions_count")]
    public int PublicReactionsCount { get; set; }

    [JsonPropertyName("page_views_count")]
    public int PageViewsCount { get; set; }

    [JsonPropertyName("published_timestamp")]
    public DateTime PublishedTimestamp { get; set; }

    [JsonPropertyName("body_markdown")]
    public required string BodyMarkdown { get; set; }

    [JsonPropertyName("positive_reactions_count")]
    public int PositiveReactionsCount { get; set; }

    [JsonPropertyName("cover_image")]
    public required string CoverImage { get; set; }

    [JsonPropertyName("tag_list")]
    public required List<string> TagList { get; set; }

    [JsonPropertyName("canonical_url")]
    public required string CanonicalUrl { get; set; }

    [JsonPropertyName("reading_time_minutes")]
    public int ReadingTimeMinutes { get; set; }

    [JsonPropertyName("user")]
    public required User User { get; set; }
}