using System.Numerics;
using System.Text.Json.Serialization;

namespace HospitalClient.Models
{
    public class Doctor
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("clinic")]
        public Clinic Clinic { get; set; }
        [JsonPropertyName("hospital")]

        public Hospital Hospital { get; set; }

        public ICollection<Appointment>? Appointments { get; set; }
    }
}
