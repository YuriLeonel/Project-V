namespace API.Models.DTO
{
    public class TokenDTO
    {
        public string Access_Token { get; set; } = string.Empty;
        public DateTime Expires_In { get; set; }
        public string Refresh_Token { get; set; } = string.Empty;
    }
}
