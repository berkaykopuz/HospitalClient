using HospitalClient.Models;
using System.ComponentModel.DataAnnotations;

namespace HospitalClient.ViewModels
{
    public class DoctorViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }
        public int SelectedClinicId {  get; set; }
        public int SelectedHospitalId { get; set; }
        public List<Clinic>? Clinics { get; set; }
        public List<Hospital>? Hospitals { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }

    }
}
