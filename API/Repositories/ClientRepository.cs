using API.Database;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ProjectVContext _db;

        public ClientRepository(ProjectVContext db)
        {
            _db = db;
        }

        public List<Client> GetAllClients()
        {
            return [.. _db.Clients];
        }

        public Client GetClient(int Id)
        {
            return _db.Clients.AsNoTracking().FirstOrDefault(c => c.IdClient == Id);
        }
        
        public Client GetClientByEmail(string Email)
        {
            return _db.Clients.AsNoTracking().FirstOrDefault(c => c.Email == Email);
        }

        public void PostClient(Client client)
        {
            _db.Clients.Add(client);
            _db.SaveChanges();
        }

        public void PatchClient(Client client)
        {
            _db.Clients.Update(client);
            _db.SaveChanges();
        }

        public void DeleteClient(Client client)
        {         
            _db.Clients.Remove(client);
            _db.SaveChanges();
        }
    }
}
