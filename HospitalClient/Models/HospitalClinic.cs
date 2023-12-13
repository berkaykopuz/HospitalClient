namespace HospitalClient.Models
{
    public class HospitalClinic
    {
        public int HospitalId { get; set; }
        public int ClinicId { get; set; }

        public Clinic Clinic { get; set; }
        public Hospital Hospital { get; set; }

    }
}
