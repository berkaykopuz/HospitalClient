using System.Text.Json.Serialization;

namespace HospitalClient.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        public Doctor Doctor { get; set; }
        public Citizen Citizen { get; set; }

    }
}
