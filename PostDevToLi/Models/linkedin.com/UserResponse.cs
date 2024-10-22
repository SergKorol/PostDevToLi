using System.Text.Json.Serialization;

namespace PostDevToLi.Models.linkedin.com;

public class UserResponse
{
    [JsonPropertyName("sub")]
    public required string Sub { get; set; }
}