using System.Text.Json.Serialization;

namespace PostDevToLi.Models.linkedin.com;

public class SpecificContent
{
    [JsonPropertyName("com.linkedin.ugc.ShareContent")]
    public ShareContent ShareContent { get; set; }
}