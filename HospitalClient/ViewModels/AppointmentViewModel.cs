using HospitalClient.Models;

namespace HospitalClient.ViewModels
{
    public class AppointmentViewModel
    {

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public List<Doctor>? Doctors { get; set; }
        public List<Clinic>? Clinics { get; set; }
        public List<Hospital>? Hospitals { get; set; }
        public int SelectedDoctorId { get; set; }
        public int SelectedClinicId { get;set; }
        public int SelectedHospitalId { get; set; }
        

    }
}
