using API.Models.Enums;

namespace API.Models.DTO
{
    public class ClientDTO
    {
        public int IdClient { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public ClientTypeEnum ClientType { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class ClientLoginDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class ClientTokenDTO
    {
        public ClientDTO Client { get; set; } = new ClientDTO();
        public Token Token { get; set; } = new Token();
    }
}
