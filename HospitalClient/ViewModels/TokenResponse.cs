using System.Text.Json.Serialization;

namespace HospitalClient.ViewModels
{
    public class TokenResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("roles")]
        public List<string> Roles { get; set; }
    }
}
