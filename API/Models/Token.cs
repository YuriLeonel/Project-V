namespace API.Models
{
    public class Token
    {
        public int IdToken { get; set; }
        public string Refresh_Token { get; set; } = string.Empty;
        public DateTime Expires_In { get; set; }
        public bool Used { get; set; }
        public int IdClient { get; set; }

        public Client Client { get; set; } = new Client();
    }
}