using System.Text.Json.Serialization;

namespace PostDevToLi.Models.linkedin.com;

public class Media
{
    [JsonPropertyName("status")]
    public string Status => "READY";

    [JsonPropertyName("description")]
    public TextBlock? Description { get; set; }

    [JsonPropertyName("originalUrl")]
    public string OriginalUrl { get; set; }

    [JsonPropertyName("title")]
    public TextBlock? Title { get; set; }
    
    [JsonPropertyName("thumbnails")]
    public Thumbnail[]? Thumbnails { get; set; }
}