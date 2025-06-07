namespace API.Models
{
    public class Token
    {
        public string Access_Token { get; set; } = string.Empty;
        public TimeSpan Expires_In { get; set; }
    }
}