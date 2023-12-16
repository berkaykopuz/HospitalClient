using Microsoft.AspNetCore.Identity;

namespace HospitalClient.Models
{
    public class Citizen : IdentityUser
    {
        public ICollection<Appointment>? Appointments { get; set; }
    }
}
