using System.Text.Json.Serialization;

namespace PostDevToLi.Models.dev.to;

public class User
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("username")]
    public required string Username { get; set; }

    [JsonPropertyName("twitter_username")]
    public required string TwitterUsername { get; set; }

    [JsonPropertyName("github_username")]
    public required string GithubUsername { get; set; }

    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("website_url")]
    public required string WebsiteUrl { get; set; }

    [JsonPropertyName("profile_image")]
    public required string ProfileImage { get; set; }

    [JsonPropertyName("profile_image_90")]
    public required string ProfileImage90 { get; set; }
}