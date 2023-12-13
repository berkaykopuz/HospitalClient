﻿namespace HospitalClient.Models
{
    public class Clinic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<HospitalClinic>? HospitalClinics { get; set; }
    }
}
