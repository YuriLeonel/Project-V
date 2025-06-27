using API.Models.Enums;

namespace API.Models
{
    public class Client : EntityBase
    {
        public int IdClient { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Password { get; set; }
        public ClientTypeEnum ClientType { get; set; }

        public Company? OwnedCompany { get; set; }
        public ICollection<Service>? ServicesProvides { get; set; }
        public ICollection<Schedule> Schedules { get; set; } = [];
        public ICollection<CompanyClients> CompanyClients { get; set; } = [];
    }
}
