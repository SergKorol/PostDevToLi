using System.Text.Json.Serialization;

namespace PostDevToLi.Models.linkedin.com;

public class LinkedInPost
{
    [JsonPropertyName("author")]
    public string Author { get; set; }

    [JsonPropertyName("lifecycleState")] public string LifecycleState => "PUBLISHED";

    [JsonPropertyName("specificContent")]
    public SpecificContent SpecificContent { get; set; }

    [JsonPropertyName("visibility")]
    public Visibility Visibility { get; set; }
}