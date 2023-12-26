using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HospitalClient.Models
{
    public class Timing
    {
        public int Id { get; set; }
        [JsonPropertyName("workDay")]
        [DataType(DataType.Date)]
        public DateTime WorkDay { get; set; }
        [JsonPropertyName("shiftStart")]
        public int ShiftStart { get; set; }
        [JsonPropertyName("shiftEnd")]
        public int ShiftEnd { get; set; }
        public Doctor Doctor { get; set; }
    }
}
