namespace API.Models.DTO
{
    public class ClientDTO
    {
        public int IdClient { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class ClientLoginDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class ClientTokenDTO
    {
        public Client Client { get; set; } = new Client();
        public Token Token { get; set; } = new Token();
    }
}
