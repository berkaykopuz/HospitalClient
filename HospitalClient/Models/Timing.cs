using System.ComponentModel.DataAnnotations;

namespace HospitalClient.Models
{
    public class Timing
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime WorkDay { get; set; }
        public int ShiftStart { get; set; }
        public int ShiftEnd { get; set; }
        public Doctor Doctor { get; set; }
    }
}
