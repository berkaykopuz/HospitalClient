using System.Numerics;

namespace HospitalClient.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Doctor>? Doctors { get; set; }

        public ICollection<HospitalClinic>? HospitalClinics { get; set; }
    }
}
