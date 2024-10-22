using System.Text.Json.Serialization;

namespace PostDevToLi.Models.linkedin.com;

public class Thumbnail
{
    [JsonPropertyName("url")]
    public string Url { get; set; }
    
    [JsonPropertyName("width")]
    public int Width { get; set; }
    
    [JsonPropertyName("height")]
    public int Height { get; set; }
    
    [JsonPropertyName("altText")]
    public string AltText { get; set; }
}