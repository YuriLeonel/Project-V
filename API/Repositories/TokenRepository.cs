using API.Database;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ProjectVContext _db;

        public TokenRepository(ProjectVContext db)
        {
            _db = db;
        }

        public Token GetTokenByToken(string refreshToken)
        {
            var token = _db.Tokens.AsNoTracking().FirstOrDefault(t => t.Refresh_Token == refreshToken);

            //Fill token's client
            if (token != null)
                token.Client = _db.Clients.AsNoTracking().FirstOrDefault(c => c.IdClient == token.IdClient);

            return token;
        }

        public void PostToken(Token token)
        {
            _db.Tokens.Add(token);
            _db.SaveChanges();
        }

        public void PatchToken(Token token)
        {
            _db.Tokens.Update(token);
            _db.SaveChanges();
        }
    }
}
