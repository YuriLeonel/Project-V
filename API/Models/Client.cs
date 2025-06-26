using API.Models.Enums;

namespace API.Models
{
    public class Client
    {
        public int IdClient { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public ClientTypeEnum ClientType { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Company? OwnedCompany { get; set; }
        public ICollection<Service>? ServicesProvides { get; set; }
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public ICollection<CompanyClients> CompanyClients { get; set; } = new List<CompanyClients>();
    }
}
