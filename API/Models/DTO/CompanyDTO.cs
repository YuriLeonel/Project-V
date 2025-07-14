namespace API.Models.DTO
{
    public class CompanyDTO
    {
        public int IdCompany { get; set; }
        public string Name { get; set; } = string.Empty;
        public ClientDTO Owner { get; set; } = new ClientDTO();
    }

    public class PostCompanyDTO
    {
        public string Name { get; set; } = string.Empty;
        public int IdOwner { get; set; }
    }
}
