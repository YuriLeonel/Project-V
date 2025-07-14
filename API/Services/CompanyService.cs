using API.Models;
using API.Models.DTO;
using API.Models.Requests;
using API.Models.Responses;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using AutoMapper;
using FluentValidation;

namespace API.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<PostCompanyDTO> _validator;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper, IValidator<PostCompanyDTO> validator)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public ResponsePaginationDefault<List<CompanyDTO>> GetAllCompanies(UrlQuery query)
        {
            var companies = _companyRepository.GetAllCompanies();

            Pagination pagination = new Pagination
            {
                Page = query.Page,
                Limit = query.Limit,
                Total = companies.Count(),
                TotalPages = companies.Count() > 0 ? (int)Math.Ceiling((double)companies.Count() / query.Limit) : 0
            };

            companies = [.. companies.Skip((query.Page - 1) * query.Limit).Take(query.Limit)];

            var companiesDTO = _mapper.Map<List<Company>, List<CompanyDTO>>(companies);

            return new ResponsePaginationDefault<List<CompanyDTO>>
            {
                Status = 200,
                Message = "Success",
                Data = companiesDTO,
                Pagination = pagination
            };
        }

        public ResponseDefault<CompanyDTO> GetCompany(int Id)
        {
            var company = _companyRepository.GetCompany(Id);

            if (company == null)
                return new ResponseDefault<CompanyDTO>
                {
                    Status = 404,
                    Message = "Company not found",
                    Errors = [$"Company with code {Id} not found"]
                };

            var companyDTO = _mapper.Map<Company, CompanyDTO>(company);
            return new ResponseDefault<CompanyDTO>
            {
                Status = 200,
                Message = "Success",
                Data = companyDTO
            };
        }

        public ResponseDefault<CompanyDTO> PostCompany(PostCompanyDTO companyDTO)
        {
            var result = _validator.Validate(companyDTO);

            if (!result.IsValid)
            {
                List<string> errors = [];
                foreach (var error in result.Errors)
                    errors.Add(error.ErrorMessage);

                return new ResponseDefault<CompanyDTO>
                {
                    Status = 400,
                    Message = "Register company failed",
                    Errors = errors,
                };
            }

            var company = _mapper.Map<PostCompanyDTO, Company>(companyDTO);
            company.Active = true;
            company.CreatedAt = DateTime.UtcNow;

            _companyRepository.PostCompany(company);

            List<int> ids = [company.IdCompany];
            _companyRepository.PostCompanyClient(company.IdOwner, ids);

            var companyResponse = _mapper.Map<Company, CompanyDTO>(company);

            return new ResponseDefault<CompanyDTO>
            {
                Status = 201,
                Message = "Company registered",
                Data = companyResponse
            };
        }

        public ResponseDefault<CompanyDTO> PatchCompany(int Id, PostCompanyDTO companyDTO)
        {
            var obj = _companyRepository.GetCompany(Id);
            if (obj == null)
                return new ResponseDefault<CompanyDTO>
                {
                    Status = 404,
                    Message = "Company not found",
                    Errors = [$"Company with code {Id} not found"]
                };

            var company = new Company
            {
                IdCompany = obj.IdCompany,
                Name = string.IsNullOrEmpty(companyDTO.Name.Trim()) ? obj.Name : companyDTO.Name,
                IdOwner = companyDTO.IdOwner == 0 ? obj.IdOwner : companyDTO.IdOwner,
                CreatedAt = obj.CreatedAt,
                UpdatedAt = DateTime.UtcNow,
                Active = obj.Active
            };

            _companyRepository.PatchCompany(company);

            var companyResponse = _mapper.Map<Company, CompanyDTO>(company);

            return new ResponseDefault<CompanyDTO>
            {
                Status = 200,
                Message = "Company changed successfully",
                Data = companyResponse
            };
        }

        public ResponseDefault<CompanyDTO> DeleteCompany(int Id)
        {
            var company = _companyRepository.GetCompany(Id);
            if (company == null)
                return new ResponseDefault<CompanyDTO>
                {
                    Status = 404,
                    Message = "Company not found",
                    Errors = [$"Company with code {Id} not found"]
                };

            _companyRepository.DeleteCompany(company);

            return new ResponseDefault<CompanyDTO>
            {
                Status = 204,
                Message = "Company deleted successfully"
            };
        }
    }
}
