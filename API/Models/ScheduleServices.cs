namespace API.Models
{
    public class ScheduleServices
    {
        public int Id { get; set; }
        public int IdSchedule { get; set; }
        public int IdService { get; set; }

        public Schedule Schedule { get; set; } = new Schedule();
        public Service Service { get; set; } = new Service();
    }
}
