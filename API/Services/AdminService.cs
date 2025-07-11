using API.Models;
using API.Models.DTO;
using API.Models.Requests;
using API.Models.Responses;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace API.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<PostUserDTO> _adminValidator;
        private readonly PasswordHasher<Client> _hasher = new();        

        public AdminService(IAdminRepository adminRepository, IMapper mapper, IValidator<PostUserDTO> validator)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
            _adminValidator = validator;
        }

        public ResponsePaginationDefault<List<ClientDTO>> GetAllAdmins(UrlQuery query)
        {
            var admins = _adminRepository.GetAllAdmins();

            Pagination pagination = new Pagination
            {
                Page = query.Page,
                Limit = query.Limit,
                Total = admins.Count(),
                TotalPages = admins.Count() > 0 ? (int)Math.Ceiling((double)admins.Count() / query.Limit) : 0
            };

            admins = [.. admins.Skip((query.Page - 1) * query.Limit).Take(query.Limit)];

            var adminDTO = _mapper.Map<List<Client>, List<ClientDTO>>(admins);

            return new ResponsePaginationDefault<List<ClientDTO>>
            {
                Status = 200,
                Message = "Success",
                Data = adminDTO,
                Pagination = pagination
            };
        }

        public ResponseDefault<CompleteAdminDTO> GetAdmin(int Id)
        {
            var admin = _adminRepository.GetCompleteAdmin(Id);

            if (admin == null)
                return new ResponseDefault<CompleteAdminDTO>
                {
                    Status = 404,
                    Message = "User not found",
                    Errors = [$"User with code {Id} not found"]
                };

            var adminDTO = _mapper.Map<Client, CompleteAdminDTO>(admin);
            return new ResponseDefault<CompleteAdminDTO>
            {
                Status = 200,
                Message = "Success",
                Data = adminDTO
            };
        }

        public ResponseDefault<ClientDTO> PostAdmin(PostUserDTO adminDTO)
        {
            var result = _adminValidator.Validate(adminDTO);

            if (!result.IsValid)
            {
                List<string> errors = [];
                foreach (var error in result.Errors)
                    errors.Add(error.ErrorMessage);

                return new ResponseDefault<ClientDTO>
                {
                    Status = 400,
                    Message = "Register user failed",
                    Errors = errors,
                };
            }

            if (_adminRepository.GetAdminByEmail(adminDTO.Email) != null)
                return new ResponseDefault<ClientDTO>
                {
                    Status = 400,
                    Message = "User already exists",
                    Errors = [$"User already registered with email {adminDTO.Email}"]
                };

            var admin = _mapper.Map<PostUserDTO, Client>(adminDTO);

            admin.Password = _hasher.HashPassword(admin, admin.Password);
            admin.Active = true;
            admin.CreatedAt = DateTime.UtcNow;
            admin.ClientType = Models.Enums.ClientTypeEnum.Admin;

            _adminRepository.PostAdmin(admin);

            var adminResponse = _mapper.Map<Client, ClientDTO>(admin);

            return new ResponseDefault<ClientDTO>
            {
                Status = 201,
                Message = "User registered",
                Data = adminResponse
            };
        }

        public ResponseDefault<ClientDTO> PatchAdmin(int Id, PostUserDTO adminDTO)
        {
            var obj = _adminRepository.GetAdmin(Id);
            if (obj == null)
                return new ResponseDefault<ClientDTO>
                {
                    Status = 404,
                    Message = "User not found",
                    Errors = [$"User witth code {Id} not found"]
                };

            var client = new Client
            {
                IdClient = obj.IdClient,
                Name = string.IsNullOrEmpty(adminDTO.Name.Trim()) ? obj.Name : adminDTO.Name,
                Email = string.IsNullOrEmpty(adminDTO.Email.Trim()) ? obj.Email : adminDTO.Email,
                ClientType = obj.ClientType,
                CreatedAt = obj.CreatedAt,
                UpdatedAt = DateTime.UtcNow,
                Active = obj.Active
            };

            client.Password = string.IsNullOrEmpty(adminDTO.Password.Trim()) ? obj.Password : _hasher.HashPassword(client, adminDTO.Password);

            _adminRepository.PatchAdmin(client);

            var adminResponse = _mapper.Map<Client, ClientDTO>(client);

            return new ResponseDefault<ClientDTO>
            {
                Status = 200,
                Message = "User changed successfully",
                Data = adminResponse
            };
        }

        public ResponseDefault<ClientDTO> DeleteAdmin(int Id)
        {
            var admin = _adminRepository.GetAdmin(Id);
            if (admin == null)
                return new ResponseDefault<ClientDTO>
                {
                    Status = 404,
                    Message = "User not found",
                    Errors = [$"User with code {Id} not found"]
                };

            _adminRepository.DeleteAdmin(admin);

            return new ResponseDefault<ClientDTO>
            {
                Status = 204,
                Message = "User deleted successfully"
            };
        }
    }
}
