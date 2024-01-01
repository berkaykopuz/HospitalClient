using System.Text.Json.Serialization;

namespace HospitalClient.Models
{
    public class Clinic
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("doctors")]
        public ICollection<Doctor>? Doctors { get; set; }
        public ICollection<HospitalClinic>? HospitalClinics { get; set; }
    }
}
