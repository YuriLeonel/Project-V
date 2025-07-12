namespace API.Models
{
    public class Reschedule
    {
        public int IdReschedule { get; set; }
        public string RescheduleReason { get; set; } = string.Empty;
        public int IdSchedule { get; set; }

        public Schedule Schedule { get; set; } = new Schedule();
    }
}
