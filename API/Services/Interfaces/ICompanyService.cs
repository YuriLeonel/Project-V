using API.Models.DTO;
using API.Models.Requests;
using API.Models.Responses;

namespace API.Services.Interfaces
{
    public interface ICompanyService
    {
        public ResponsePaginationDefault<List<CompanyDTO>> GetAllCompanies(UrlQuery query);
        public ResponseDefault<CompanyDTO> GetCompany(int Id);
        public ResponseDefault<CompanyDTO> PostCompany(PostCompanyDTO companyDTO);
        public ResponseDefault<CompanyDTO> PatchCompany(int Id, PostCompanyDTO companyDTO);
        public ResponseDefault<CompanyDTO> DeleteCompany(int Id);
    }
}
