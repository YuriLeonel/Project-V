namespace API.Models
{
    public class CompanyClients
    {
        public int Id { get; set; }
        public int IdCompany { get; set; }
        public int IdClient { get; set; }

        public Company Company { get; set; } = new Company();
        public Client Client { get; set; } = new Client();
    }
}
