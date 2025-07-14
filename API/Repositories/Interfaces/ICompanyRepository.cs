using API.Models;

namespace API.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        public List<Company> GetAllCompanies();
        public Company GetCompany(int Id);
        public Company GetCompleteCompany(int Id);
        public void PostCompany(Company company);
        public void PatchCompany(Company company);
        public void DeleteCompany(Company company);
        //==================================================================
        public void PostCompanyClient(int IdClient, List<int> IdCompanies);
        public void PatchCompanyClient(int IdClient, List<int> IdCompanies);
    }
}
