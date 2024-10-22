using System.Text.Json.Serialization;

namespace PostDevToLi.Models.linkedin.com;

public class ShareContent
{
    [JsonPropertyName("shareCommentary")]
    public TextBlock ShareCommentary { get; set; }

    [JsonPropertyName("shareMediaCategory")]
    public string ShareMediaCategory => ShareMediaCategoryType switch
    {
        ShareMediaCategoryType.None => "NONE",
        ShareMediaCategoryType.Article => "ARTICLE",
        ShareMediaCategoryType.Media => "MEDIA",
        _ => "NONE"
    };

    [JsonIgnore]
    public ShareMediaCategoryType ShareMediaCategoryType { get; set; }

    [JsonPropertyName("media")]
    public Media[] Media { get; set; }
}