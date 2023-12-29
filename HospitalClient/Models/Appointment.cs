using System.Text.Json.Serialization;

namespace HospitalClient.Models
{
    public class Appointment
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("doctor")]
        public Doctor Doctor { get; set; }
        [JsonPropertyName("citizen")]
        public Citizen Citizen { get; set; }

    }
}
