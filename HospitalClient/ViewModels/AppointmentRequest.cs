namespace HospitalClient.ViewModels
{
    public class AppointmentRequest
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public int doctorId { get; set; }
        public string citizenId {  get; set; } 

    }
}
