using API.Database;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private ProjectVContext _db;
        public AdminRepository(ProjectVContext db)
        {
            _db = db;
        }

        public List<Client> GetAllAdmins()
        {
            return [.. _db.Clients.AsNoTracking().Where(a => a.ClientType == Models.Enums.ClientTypeEnum.Admin || a.ClientType == Models.Enums.ClientTypeEnum.Owner)];
        }

        public Client GetAdmin(int Id)
        {
            return _db.Clients.AsNoTracking().FirstOrDefault(a => (a.ClientType == Models.Enums.ClientTypeEnum.Admin || a.ClientType == Models.Enums.ClientTypeEnum.Owner) && a.IdClient == Id);
        }

        public Client GetCompleteAdmin(int Id)
        {
            //var admin = _db.Clients.AsNoTracking().FirstOrDefault(a => (a.ClientType == Models.Enums.ClientTypeEnum.Admin || a.ClientType == Models.Enums.ClientTypeEnum.Owner) && a.IdClient == Id);
            var admin = GetAdmin(Id);

            //Fill admin's company
            //Search owner's company
            if (admin != null)
            {
                admin.CompanyClients = [.. _db.CompanyClients.AsNoTracking().Where(cc => cc.IdClient == admin.IdClient)];

                if (admin.ClientType == Models.Enums.ClientTypeEnum.Owner)
                    admin.OwnedCompany = _db.Companies.AsNoTracking().FirstOrDefault(c => c.IdOwner == admin.IdClient);
            }

            return admin;
        }

        public Client GetAdminByEmail(string Email)
        {
            return _db.Clients.AsNoTracking().FirstOrDefault(a => (a.ClientType == Models.Enums.ClientTypeEnum.Admin || a.ClientType == Models.Enums.ClientTypeEnum.Owner) && a.Email == Email && a.Active);
        }

        public void PostAdmin(Client admin)
        {
            _db.Clients.Add(admin);
            _db.SaveChanges();
        }

        public void PatchAdmin(Client admin)
        {
            _db.Clients.Update(admin);
            _db.SaveChanges();
        }

        public void DeleteAdmin(Client admin)
        {
            if (AdminHasCompanies(admin.IdClient))
            {
                admin.Active = false;
                admin.UpdatedAt = DateTime.UtcNow;

                _db.Clients.Update(admin);
                _db.SaveChanges();
            }
            else
            {
                _db.CompanyClients.RemoveRange(_db.CompanyClients.Where(cc => cc.IdClient == admin.IdClient));
                _db.Clients.Remove(admin);
                _db.SaveChanges();
            }
        }

        private bool AdminHasCompanies(int Id)
        {
            var admin = _db.Clients.FromSqlInterpolated($"SELECT C.IdClient FROM Clients AS C JOIN CompanyClients AS CC ON CC.IdClient = C.IdClient WHERE C.IdClient = {Id}").FirstOrDefault();

            if (admin == null)
                return false;

            return true;
        }
    }
}
