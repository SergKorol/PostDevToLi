using System.Text.Json.Serialization;

namespace PostDevToLi.Models.linkedin.com;

public class TextBlock
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
}