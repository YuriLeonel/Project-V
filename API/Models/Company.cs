namespace API.Models
{
    public class Company
    {
        public int IdCompany { get; set; }
        public string Name { get; set; } = string.Empty;
        public int IdOwner { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Client Owner { get; set; } = new Client();
        public ICollection<CompanyClients> CompanyClients { get; set; } = new List<CompanyClients>();
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
