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
            return [.. _db.Clients.AsNoTracking().Where(c => c.ClientType == Models.Enums.ClientTypeEnum.Client)];
        }

        public Client GetClient(int Id)
        {
            return _db.Clients.AsNoTracking().FirstOrDefault(c => c.ClientType == Models.Enums.ClientTypeEnum.Client && c.IdClient == Id);
        }

        public Client GetCompleteClient(int Id)
        {
            //var client = _db.Clients.AsNoTracking().FirstOrDefault(c => c.ClientType == Models.Enums.ClientTypeEnum.Client && c.IdClient == Id);
            var client = GetClient(Id);

            //Fill client's schedule from requisition date
            if (client != null)
                client.Schedules = [.. _db.Schedules.AsNoTracking().Where(s => s.IdClient == client.IdClient && s.BookedtAt >= DateTime.UtcNow)];

            return client;
        }

        public Client GetClientByEmail(string Email)
        {
            return _db.Clients.AsNoTracking().FirstOrDefault(c => c.ClientType == Models.Enums.ClientTypeEnum.Client && c.Email == Email && c.Active);
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
            if (ClientHasSchedule(client.IdClient))
            {
                client.Active = false;
                client.UpdatedAt = DateTime.UtcNow;

                _db.Clients.Update(client);
                _db.SaveChanges();
            }
            else
            {
                _db.Clients.Remove(client);
                _db.SaveChanges();
            }
        }

        private bool ClientHasSchedule(int Id)
        {
            var client = _db.Clients.FromSqlInterpolated($"SELECT C.IdClient FROM Clients AS C JOIN Schedules AS S ON S.IdClient = C.IdClient WHERE C.IdClient = {Id}").FirstOrDefault();

            if (client == null)
                return false;

            return true;
        }
    }
}
