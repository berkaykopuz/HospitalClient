using HospitalClient.Models;

namespace HospitalClient.ViewModels
{
    public class HospitalClinicViewModel
    {
        public int SelectedHospitalId { get; set; }
        public int SelectedClinicId { get; set; }

        public List<Clinic>? Clinics { get; set; }
        public List<Hospital>? Hospitals { get; set; }
    }
}
