namespace API.Models
{
    public class Company : EntityBase
    {
        public int IdCompany { get; set; }
        public string Name { get; set; } = string.Empty;
        public int IdOwner { get; set; }

        public Client Owner { get; set; } = new Client();
        public ICollection<CompanyClients> CompanyClients { get; set; } = [];
        public ICollection<Schedule> Schedules { get; set; } = [];
    }
}
