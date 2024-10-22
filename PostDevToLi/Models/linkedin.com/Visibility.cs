using System.Text.Json.Serialization;

namespace PostDevToLi.Models.linkedin.com;

public class Visibility
{
    [JsonPropertyName("com.linkedin.ugc.MemberNetworkVisibility")]
    public string MemberNetworkVisibility => VisibilityType switch
    {
        VisibilityType.Anyone => "PUBLIC",
        VisibilityType.Connections => "CONNECTIONS",
        _ => "PUBLIC"
    };
    
    [JsonIgnore]
    public VisibilityType VisibilityType { get; set; }
}