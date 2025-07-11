namespace API.Models.DTO
{
    public class ClientDTO
    {
        public int IdClient { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        //public string Password { get; set; } = string.Empty;
        //public ClientTypeEnum ClientType { get; set; }
        //public bool Active { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime? UpdatedAt { get; set; }
    }

    public class CompleteClientDTO : ClientDTO
    {
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<Schedule>? Schedules { get; set; }
    }

    public class CompleteAdminDTO : ClientDTO
    {
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Company? OwnedCompany { get; set; }
        public ICollection<CompanyClients> CompanyClients { get; set; } = [];
    }

    public class CompleteEmployeeDTO : ClientDTO
    {
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<Service>? ServicesProvides { get; set; }
        public ICollection<CompanyClients> CompanyClients { get; set; } = [];
    }

    public class PostClientDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class PostUserDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<int> CompanyId { get; set; } = [];
    }

    public class ClientLoginDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class ClientTokenDTO
    {
        public ClientDTO Client { get; set; } = new ClientDTO();
        public TokenDTO Token { get; set; } = new TokenDTO();
    }
}
