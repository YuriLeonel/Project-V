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
    public class AdminService : IAdminService
    {
        private IAdminRepository _adminRepository;
        private IMapper _mapper;
        private IValidator<PostUserDTO> _adminValidator;
        private PasswordHasher<Client> _hasher = new();

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
                Message = "Sucesso",
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
                    Message = "Usuário não encontrado",
                    Errors = [$"Usuário com código {Id} não encontrado"]
                };

            var adminDTO = _mapper.Map<Client, CompleteAdminDTO>(admin);
            return new ResponseDefault<CompleteAdminDTO>
            {
                Status = 200,
                Message = "Sucesso",
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
                    Message = "Erro ao cadastrar funcionário",
                    Errors = errors,
                };
            }

            if (_adminRepository.GetAdminByEmail(adminDTO.Email) != null)
                return new ResponseDefault<ClientDTO>
                {
                    Status = 400,
                    Message = "Usuário já existe",
                    Errors = [$"Usário já cadastrado com email {adminDTO.Email}"]
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
                Message = "Usuário criado",
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
                    Message = "Usuário não encontrado",
                    Errors = [$"Usário com código {Id} não encontrado"]
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
                Message = "Usuário alterado com sucesso",
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
                    Message = "Usuário não encontrado",
                    Errors = [$"Usuário com o código {Id} não encontrado"]
                };

            _adminRepository.DeleteAdmin(admin);

            return new ResponseDefault<ClientDTO>
            {
                Status = 204,
                Message = "Usuário deletado com sucesso"
            };
        }
    }
}
