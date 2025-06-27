namespace API.Models
{
    public class Service: EntityBase
    {
        public int IdService { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int Duration { get; set; }
        public double Price { get; set; }
        public int IdEmployee { get; set; }

        public Client Employee { get; set; } = new Client();
        public ICollection<ScheduleServices> ScheduleServices { get; set; } = [];
    }
}
