using HospitalClient.Models;
using System.ComponentModel.DataAnnotations;

namespace HospitalClient.ViewModels
{
    public class TimingViewModel
    {
        public TimingViewModel()
        {
            ShiftStarts = GenerateHourList();
            ShiftEnds = GenerateHourList();
        }

        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime WorkDay { get; set; }
        public List<int> ShiftStarts {  get; set; }
        public List<int> ShiftEnds { get; set; }
        public int SelectedShiftStart { get; set; }
        public int SelectedShiftEnd { get; set; }
        public int DoctorId { get; set; }

        private List<int> GenerateHourList()
        {
            List<int> hours = new List<int>();
            for (int i = 0; i < 24; i++)
            {
                hours.Add(i);
            }
            return hours;
        }
    }
}
