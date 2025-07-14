using API.Database;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private ProjectVContext _db;
        public EmployeeRepository(ProjectVContext db)
        {
            _db = db;
        }

        public List<Client> GetAllEmployees()
        {
            return [.. _db.Clients.AsNoTracking().Where(e => e.ClientType == Models.Enums.ClientTypeEnum.Employee)];
        }

        public Client GetEmployee(int Id)
        {
            return _db.Clients.AsNoTracking().FirstOrDefault(e => e.ClientType == Models.Enums.ClientTypeEnum.Employee && e.IdClient == Id);
        }

        public Client GetCompleteEmployee(int Id)
        {
            //var employee = _db.Clients.AsNoTracking().FirstOrDefault(e => e.ClientType == Models.Enums.ClientTypeEnum.Employee && e.IdClient == Id);
            var employee = GetEmployee(Id);

            //Fill employee's services
            //Fill employee's company
            if (employee != null)
            {
                employee.ServicesProvides = [.. _db.Services.AsNoTracking().Where(s => s.IdEmployee == employee.IdClient)];
                employee.CompanyClients = [.. _db.CompanyClients.AsNoTracking().Where(cc => cc.IdClient == employee.IdClient)];
            }

            return employee;
        }

        public Client GetEmployeeByEmail(string Email)
        {
            return _db.Clients.AsNoTracking().FirstOrDefault(e => e.ClientType == Models.Enums.ClientTypeEnum.Employee && e.Email == Email && e.Active);
        }

        public void PostEmployee(Client employee)
        {
            _db.Clients.Add(employee);
            _db.SaveChanges();
        }

        public void PatchEmployee(Client employee)
        {
            _db.Clients.Update(employee);
            _db.SaveChanges();
        }

        public void DeleteEmployee(Client employee)
        {
            if (EmployeeHasService(employee.IdClient))
            {
                employee.Active = false;
                employee.UpdatedAt = DateTime.UtcNow;

                _db.Clients.Update(employee);
                _db.SaveChanges();
            }
            else
            {
                _db.CompanyClients.RemoveRange(_db.CompanyClients.Where(cc => cc.IdClient == employee.IdClient));
                _db.Clients.Remove(employee);
                _db.SaveChanges();
            }
        }

        private bool EmployeeHasService(int Id)
        {
            var employee = _db.Clients.FromSqlInterpolated($"SELECT C.IdClient FROM Clients AS C JOIN [Services] AS S ON S.IdEmployee = C.IdClient WHERE C.IdClient = {Id}").FirstOrDefault();

            if (employee == null)
                return false;

            return true;
        }
    }
}
