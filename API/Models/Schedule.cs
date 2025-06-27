using API.Models.Enums;

namespace API.Models
{
    public class Schedule
    {
        public int IdSchedule { get; set; }
        public int IdCompany { get; set; }
        public int IdClient { get; set; }        
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public StatusScheduleEnum Staus { get; set; }
        public DateTime BookedtAt { get; set; }
        public DateTime? RescheduledtAt { get; set; }
        public bool Active { get; set; }

        public Company Company { get; set; } = new Company();
        public Client Client { get; set; } = new Client();
        public ICollection<ScheduleServices> ScheduleServices { get; set; } = [];
        public ICollection<Reschedule> Reschedules { get; set; } = [];
    }
}
