using HospitalClient.Models;
using System.ComponentModel.DataAnnotations;

namespace HospitalClient.ViewModels
{
    public class DoctorViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [Display(Name = "Doctor Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }
        public int SelectedHospitalId { get; set; }
        public List<Hospital>? Hospitals { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }

        
    }
}
