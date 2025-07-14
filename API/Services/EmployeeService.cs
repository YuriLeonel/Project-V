using API.Models;
using API.Models.DTO;
using API.Models.Requests;
using API.Models.Responses;
using API.Repositories;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<PostUserDTO> _employeeValidator;
        private readonly PasswordHasher<Client> _hasher = new();

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper, IValidator<PostUserDTO> validator, ICompanyRepository companyRepository)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _employeeValidator = validator;
            _companyRepository = companyRepository;
        }

        public ResponsePaginationDefault<List<ClientDTO>> GetAllEmployees(UrlQuery query)
        {
            var employees = _employeeRepository.GetAllEmployees();

            Pagination pagination = new Pagination
            {
                Page = query.Page,
                Limit = query.Limit,
                Total = employees.Count(),
                TotalPages = employees.Count() > 0 ? (int)Math.Ceiling((double)employees.Count() / query.Limit) : 0
            };

            employees = [.. employees.Skip((query.Page - 1) * query.Limit).Take(query.Limit)];

            var employeesDTO = _mapper.Map<List<Client>, List<ClientDTO>>(employees);

            return new ResponsePaginationDefault<List<ClientDTO>>
            {
                Status = 200,
                Message = "Success",
                Data = employeesDTO,
                Pagination = pagination
            };
        }

        public ResponseDefault<CompleteEmployeeDTO> GetEmployee(int Id)
        {
            var employee = _employeeRepository.GetCompleteEmployee(Id);

            if (employee == null)
                return new ResponseDefault<CompleteEmployeeDTO>
                {
                    Status = 404,
                    Message = "Employee not found",
                    Errors = [$"Employee with code {Id} not found"]
                };

            var employeeDTO = _mapper.Map<Client, CompleteEmployeeDTO>(employee);
            return new ResponseDefault<CompleteEmployeeDTO>
            {
                Status = 200,
                Message = "Success",
                Data = employeeDTO
            };
        }

        public ResponseDefault<ClientDTO> PostEmployee(PostUserDTO employeeDTO)
        {
            var employee = new Client();

            if (string.IsNullOrEmpty(employeeDTO.Email.Trim()))
            {
                var result = _employeeValidator.Validate(employeeDTO, opt =>
                {
                    opt.IncludeProperties(x => x.Name);
                    opt.IncludeProperties(x => x.CompanyId);
                });

                if (!result.IsValid)
                {
                    List<string> errors = [];
                    foreach (var error in result.Errors)
                        errors.Add(error.ErrorMessage);

                    return new ResponseDefault<ClientDTO>
                    {
                        Status = 400,
                        Message = "Register employee failed",
                        Errors = errors,
                    };
                }

                foreach (var id in employeeDTO.CompanyId)
                    if (_companyRepository.GetCompany(id) == null)
                        return new ResponseDefault<ClientDTO>
                        {
                            Status = 400,
                            Message = "Company not found",
                            Errors = [$"Company {id} not found"]
                        };

                employee = _mapper.Map<PostUserDTO, Client>(employeeDTO);

                employee.Active = true;
                employee.CreatedAt = DateTime.UtcNow;
                employee.ClientType = Models.Enums.ClientTypeEnum.Employee;
                employee.Email = string.Empty;
                employee.Password = string.Empty;
            }
            else
            {
                var result = _employeeValidator.Validate(employeeDTO);

                if (!result.IsValid)
                {
                    List<string> errors = [];
                    foreach (var error in result.Errors)
                        errors.Add(error.ErrorMessage);

                    return new ResponseDefault<ClientDTO>
                    {
                        Status = 400,
                        Message = "Register employee failed",
                        Errors = errors,
                    };
                }

                if (_employeeRepository.GetEmployeeByEmail(employeeDTO.Email) != null)
                    return new ResponseDefault<ClientDTO>
                    {
                        Status = 400,
                        Message = "Employee already exists",
                        Errors = [$"Employee already registered with email {employeeDTO.Email}"]
                    };

                foreach (var id in employeeDTO.CompanyId)
                    if (_companyRepository.GetCompany(id) == null)
                        return new ResponseDefault<ClientDTO>
                        {
                            Status = 400,
                            Message = "Company not found",
                            Errors = [$"Company {id} not found"]
                        };

                employee = _mapper.Map<PostUserDTO, Client>(employeeDTO);

                employee.Password = _hasher.HashPassword(employee, employee.Password);
                employee.Active = true;
                employee.CreatedAt = DateTime.UtcNow;
                employee.ClientType = Models.Enums.ClientTypeEnum.Employee;
            }

            _employeeRepository.PostEmployee(employee);
            _companyRepository.PostCompanyClient(employee.IdClient, employeeDTO.CompanyId);

            var employeeResponse = _mapper.Map<Client, ClientDTO>(employee);

            return new ResponseDefault<ClientDTO>
            {
                Status = 201,
                Message = "Employee registered",
                Data = employeeResponse
            };
        }

        public ResponseDefault<ClientDTO> PatchEmployee(int Id, PostUserDTO employeeDTO)
        {
            var obj = _employeeRepository.GetEmployee(Id);
            if (obj == null)
                return new ResponseDefault<ClientDTO>
                {
                    Status = 404,
                    Message = "Employee not found",
                    Errors = [$"Employee with code {Id} not found"]
                };

            foreach (var id in employeeDTO.CompanyId)
                if (_companyRepository.GetCompany(id) == null)
                    return new ResponseDefault<ClientDTO>
                    {
                        Status = 400,
                        Message = "Company not found",
                        Errors = [$"Company {id} not found"]
                    };

            var client = new Client
            {
                IdClient = obj.IdClient,
                Name = string.IsNullOrEmpty(employeeDTO.Name.Trim()) ? obj.Name : employeeDTO.Name,
                Email = string.IsNullOrEmpty(employeeDTO.Email.Trim()) ? obj.Email : employeeDTO.Email,
                ClientType = obj.ClientType,
                CreatedAt = obj.CreatedAt,
                UpdatedAt = DateTime.UtcNow,
                Active = obj.Active
            };

            client.Password = string.IsNullOrEmpty(employeeDTO.Password.Trim()) ? obj.Password : _hasher.HashPassword(client, employeeDTO.Password);

            _employeeRepository.PatchEmployee(client);
            _companyRepository.PatchCompanyClient(client.IdClient, employeeDTO.CompanyId);

            var employeeResponse = _mapper.Map<Client, ClientDTO>(client);

            return new ResponseDefault<ClientDTO>
            {
                Status = 200,
                Message = "Employee changed successfully",
                Data = employeeResponse
            };
        }

        public ResponseDefault<ClientDTO> DeleteEmployee(int Id)
        {
            var employee = _employeeRepository.GetEmployee(Id);
            if (employee == null)
                return new ResponseDefault<ClientDTO>
                {
                    Status = 404,
                    Message = "Employee not found",
                    Errors = [$"Employee with code {Id} not found"]
                };

            _employeeRepository.DeleteEmployee(employee);

            return new ResponseDefault<ClientDTO>
            {
                Status = 204,
                Message = "Employee deleted successfully"
            };
        }
    }
}
