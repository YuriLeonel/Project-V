using API.Database;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ProjectVContext _db;

        public CompanyRepository(ProjectVContext db)
        {
            _db = db;
        }

        public List<Company> GetAllCompanies()
        {
            return [.. _db.Companies.AsNoTracking()];
        }

        public Company GetCompany(int Id)
        {
            return _db.Companies.AsNoTracking().FirstOrDefault(c => c.IdCompany == Id);
        }

        public Company GetCompleteCompany(int Id)
        {
            var company = GetCompany(Id);

            //Fill company's employee
            //Fill company's owner
            if (company != null)
            {
                company.Owner = _db.Clients.AsNoTracking().FirstOrDefault(o => o.IdClient == company.IdOwner);
                company.CompanyClients = [.. _db.CompanyClients.AsNoTracking()];
            }

            return company;
        }

        public void PostCompany(Company company)
        {
            _db.Companies.Add(company);
            _db.SaveChanges();
        }

        public void PatchCompany(Company company)
        {
            _db.Companies.Update(company);
            _db.SaveChanges();
        }

        public void DeleteCompany(Company company)
        {
            if (CompanyHasEmployee(company.IdCompany))
            {
                company.Active = false;
                company.UpdatedAt = DateTime.UtcNow;

                _db.Companies.Update(company);
                _db.SaveChanges();
            }
            else
            {
                _db.Companies.Remove(company);
                _db.SaveChanges();
            }
        }

        private bool CompanyHasEmployee(int Id)
        {
            var company = _db.Companies.FromSqlInterpolated($"SELECT C.IdCompany FROM Companies AS C JOIN CompanyClients AS CC ON CC.IdCompany = C.IdCompany WHERE C.IdCompany = {Id}").FirstOrDefault();

            if (company == null)
                return false;

            return true;
        }

        //==================================================================

        public void PostCompanyClient(int IdClient, List<int> IdCompanies)
        {
            IdCompanies.ForEach(id =>
            {
                var companyClient = new CompanyClients
                {
                    IdClient = IdClient,
                    IdCompany = id
                };

                _db.CompanyClients.Add(companyClient);
            });
            _db.SaveChanges();
        }

        public void PatchCompanyClient(int IdClient, List<int> IdCompanies)
        {
            _db.CompanyClients.RemoveRange(_db.CompanyClients.Where(cc => cc.IdClient == IdClient));
            _db.SaveChanges();

            PostCompanyClient(IdClient, IdCompanies);
        }
    }
}
