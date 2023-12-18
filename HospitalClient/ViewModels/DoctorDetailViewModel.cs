using HospitalClient.Models;
using System.ComponentModel.DataAnnotations;

namespace HospitalClient.ViewModels
{
    public class DoctorDetailViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }
        public Hospital Hospital { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
    }
}
