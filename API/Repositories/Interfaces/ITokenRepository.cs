using API.Models;

namespace API.Repositories.Interfaces
{
    public interface ITokenRepository
    {
        public Token GetTokenByToken(string refreshToken);
        public void PostToken(Token token);
        public void PatchToken(Token token);
    }
}
